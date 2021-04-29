using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Common.Bus.Interfaces;
using Zero99Lotto.SRC.Common.Handlers;
using Zero99Lotto.SRC.Common.Messages.Events.Draw;
using Zero99Lotto.SRC.Services.Draws.API.Core.Domain.Repositories;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Services.Interfaces;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Handlers
{
    public class ScheduleUpdatedHandler : IDomainEventHandler<Messages.DomainEvents.ScheduleUpdated>
    {
        private readonly IHandler _handler;
        private readonly IBusPublisher _busPublisher;
        private readonly ILogger<ScheduleUpdatedHandler> _logger;
        private readonly IDateEstimatorProvider _dateEstimatorProvider;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IDrawingService _drawingService;

        public ScheduleUpdatedHandler(IHandler handler, IBusPublisher busPublisher,
            ILogger<ScheduleUpdatedHandler> logger, IDateEstimatorProvider dateEstimatorProvider,
            IScheduleRepository scheduleRepository, IDrawingService drawingService)
        {
            _handler = handler;
            _busPublisher = busPublisher;
            _logger = logger;
            _dateEstimatorProvider = dateEstimatorProvider;
            _scheduleRepository = scheduleRepository;
            _drawingService = drawingService;
        }

        public async Task HandleAsync(Messages.DomainEvents.ScheduleUpdated @event)
        {
            var newDrawingCreated = false;
            await _handler
              .Handle(async () =>
              {
                  _logger.LogInformation($"[{nameof(ScheduleUpdatedHandler)}] - Updating drawing for schedule {@event.Id}.");
                  var schedule = await _scheduleRepository.GetAsync(@event.Id);
                  var daysOfWeek = schedule.Days.Select(x => x.DayOfWeek).ToArray();
                  var estimatedDay = _dateEstimatorProvider.GetClosestDate(daysOfWeek, schedule.StartTime);
                  _logger.LogInformation($"[{nameof(ScheduleUpdatedHandler)}] - Estimated date: {estimatedDay}. Provided days {string.Join(',', daysOfWeek)}, time {schedule.StartTime}.");
                  if (schedule.HasActiveDrawing())
                  {
                      var drawing = schedule.GetActiveDrawing();
                      drawing.UpdateStartDate(estimatedDay);
                  }
                  else
                  {
                      var drawing = _drawingService.CreateDrawing(estimatedDay);
                      schedule.AddDrawing(await drawing);
                      schedule.MarkAsActive(await drawing);
                      newDrawingCreated = true;
                  }

                  await _scheduleRepository.UpdateAsync(schedule);
              })
              .OnSuccess(async () =>
              {
                  var schedule = await _scheduleRepository.GetAsync(@event.Id);
                  var drawing = schedule.GetActiveDrawing();

                  _logger.LogInformation($"[{nameof(ScheduleUpdatedHandler)}] - Drawing updated ({drawing.Id}) for schedule {schedule.Id}.");

                  if (newDrawingCreated)
                      await _busPublisher.PublishAsync(new DrawingCreated(drawing.Id, drawing.StartDate));
                  else
                      await _busPublisher.PublishAsync(new DrawingUpdated(drawing.Id, drawing.StartDate,
                          drawing.Numbers.Where(x => !x.Extra).Select(x => x.Value).ToArray(),
                          drawing.Numbers.Where(x => x.Extra).Select(x => x.Value).ToArray()));


              })
              .OnCustomError(async (ex) =>
              {
                  _logger.LogError($"[{nameof(ScheduleCreatedHandler)}] - Custom error occurred while updating drawing for schedule. {ex.Message}");
                  await Task.FromException(ex);
              })
              .OnError(async (ex) =>
              {
                  _logger.LogError($"[{nameof(ScheduleCreatedHandler)}] - Error occurred while updating drawing for schedule. Message: {ex.Message}. StackTrace: {ex.StackTrace}");
                  await Task.FromException(ex);
              })
              .ExecuteAsync().ConfigureAwait(false);
        }
    }
}
