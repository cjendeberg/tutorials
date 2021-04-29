using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Common.Bus.Interfaces;
using Zero99Lotto.SRC.Common.Handlers;
using Zero99Lotto.SRC.Common.Messages.Events.Draw;
using Zero99Lotto.SRC.Services.Draws.API.Core.Domain.Repositories;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Messages.DomainEvents;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Services.Interfaces;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Handlers
{
    public class ScheduleCreatedHandler : IDomainEventHandler<ScheduleCreated>
    {
        private readonly IHandler _handler;
        private readonly IBusPublisher _busPublisher;
        private readonly IDrawingService _drawingService;
        private readonly IDateEstimatorProvider _dateEstimatorProvider;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly ILogger<ScheduleCreated> _logger;

        public ScheduleCreatedHandler(IDrawingService drawingService, IScheduleRepository scheduleRepository,
            IHandler handler, IDateEstimatorProvider dateEstimatorProvider, IBusPublisher busPublisher,
            ILogger<ScheduleCreated> logger)
        {
            _drawingService = drawingService;
            _handler = handler;
            _busPublisher = busPublisher;
            _dateEstimatorProvider = dateEstimatorProvider;
            _scheduleRepository = scheduleRepository;
            _logger = logger;
        }

        public Task HandleAsync(ScheduleCreated @event)
            =>
             _handler
                .Handle(async () =>
                {
                    _logger.LogInformation($"[{nameof(ScheduleCreatedHandler)}] - Creating first drawing for new schedule ({@event.Id}) and marking as active the closest");
                    var schedule = await _scheduleRepository.GetAsync(@event.Id);
                    var daysOfWeek = schedule.Days.Select(x => x.DayOfWeek).ToArray();
                    var estimatedDay = _dateEstimatorProvider.GetClosestDate(daysOfWeek, schedule.StartTime);
                    var drawing = _drawingService.CreateDrawing(estimatedDay);
                    schedule.AddDrawing(await drawing);
                    schedule.MarkAsActive(await drawing);

                    await _scheduleRepository.UpdateAsync(schedule);
                })
                .OnSuccess(async () =>
                {
                    _logger.LogInformation($"[{nameof(ScheduleCreatedHandler)}] - First drawing created for schedule {@event.Id}.");
                    var schedule = await _scheduleRepository.GetAsync(@event.Id);
                    var activeDrawing = schedule.GetActiveDrawing();
                    await _busPublisher.PublishAsync(new DrawingCreated(activeDrawing.Id, activeDrawing.StartDate));
                })
                .OnCustomError(async (ex) =>
                {
                    _logger.LogError($"[{nameof(ScheduleCreatedHandler)}] - Custom error occurred while creating first drawing for new schedule. {ex.Message}");
                    await Task.FromException(ex);
                })
                .OnError(async (ex) =>
                {
                    _logger.LogError($"[{nameof(ScheduleCreatedHandler)}] - Error occurred while creating first drawing for new schedule. Message: {ex.Message}. StackTrace: {ex.StackTrace}");
                    await Task.FromException(ex);
                })
                .ExecuteAsync();
        
    }
}
