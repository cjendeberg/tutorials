using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Common.Bus.Interfaces;
using Zero99Lotto.SRC.Common.Handlers;
using Zero99Lotto.SRC.Common.Messages.Events.Game;
using Zero99Lotto.SRC.Services.Draws.API.Core.Domain.Repositories;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Messages.DomainEvents;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Services.Interfaces;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Setings;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Handlers
{
    public class ShutdownDisabledHandler : IEventHandler<ShutdownDisabled>
    {
        private readonly IHandler _handler;
        private readonly IBusPublisher _busPublisher;
        private readonly IDrawingService _drawingService;
        private readonly IDateEstimatorProvider _dateEstimatorProvider;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly ILogger<ShutdownDisabledHandler> _logger;
        private readonly DrawingSettings _drawingSettings;

        public ShutdownDisabledHandler(IDrawingService drawingService, IScheduleRepository scheduleRepository,
            IHandler handler, IDateEstimatorProvider dateEstimatorProvider, IBusPublisher busPublisher,
            ILogger<ShutdownDisabledHandler> logger, DrawingSettings drawingSettings)
        {
            _drawingService = drawingService;
            _handler = handler;
            _busPublisher = busPublisher;
            _dateEstimatorProvider = dateEstimatorProvider;
            _scheduleRepository = scheduleRepository;
            _logger = logger;
            _drawingSettings = drawingSettings;
        }

        public Task HandleAsync(ShutdownDisabled @event)
        =>
            _handler
               .Handle(async () =>
               {
                   _logger.LogInformation($"[{nameof(ShutdownDisabledHandler)}] - shutdown end. Prepare next drawing.");

                   var schedule = await _scheduleRepository.GetAsync();

                   if (schedule == null)
                       throw new ServiceException("Cannot create a drawing because there is no schedule set.");

                   if(schedule.HasActiveDrawing())
                       throw new ServiceException("Cannot create a drawing because there is already an active drawing.");

                   var daysOfWeek = schedule.Days.Select(x => x.DayOfWeek).ToArray();

                   if (daysOfWeek.Length > 0)
                   {
                       _logger.LogInformation($"[{nameof(ShutdownDisabledHandler)}] - Estimating closest date.");
                       var estimatedDay = _drawingSettings.IntervalInMinutes.HasValue ?
                                                DateTime.UtcNow.AddMinutes(_drawingSettings.IntervalInMinutes.Value)
                                                : _dateEstimatorProvider.GetClosestDate(daysOfWeek, schedule.StartTime);

                       _logger.LogInformation($"[{nameof(ShutdownDisabledHandler)}] - Creating next drawing for schedule.");
                       var drawing = _drawingService.CreateDrawing(estimatedDay);
                       schedule.AddDrawing(await drawing);

                       _logger.LogInformation($"[{nameof(ShutdownDisabledHandler)}] - Marking as active the closes drawing.");
                       schedule.MarkAsActive(await drawing);
                       await _scheduleRepository.UpdateAsync(schedule);
                   }
                   else
                       throw new ServiceException("Cannot create drawing because there are no schedule days set.");

               })
               .OnSuccess(async () =>
               {
                   var schedule = await _scheduleRepository.GetAsync();
                   var activeDrawing = schedule.GetActiveDrawing();

                   _logger.LogInformation($"[{nameof(ShutdownDisabledHandler)}] - New drawing created ({activeDrawing.Id}).");

                   await _busPublisher.PublishAsync(new Common.Messages.Events.Draw.DrawingCreated(
                       activeDrawing.Id, activeDrawing.StartDate));
               })
               .OnCustomError(async (ex) =>
               {
                   _logger.LogError($"[{nameof(ShutdownDisabledHandler)}] - Custom error occurred while creating new drawing after shutdown end. {ex.Message}");
                   await Task.FromException(ex);
               })
               .OnError(async (ex) =>
               {
                   _logger.LogError($"[{nameof(ShutdownDisabledHandler)}] - Error occurred while creating new drawing after shutdown end. Message: {ex.Message}. StackTrace: {ex.StackTrace}");
                   await Task.FromException(ex);
               })
               .ExecuteAsync();

    }
}
