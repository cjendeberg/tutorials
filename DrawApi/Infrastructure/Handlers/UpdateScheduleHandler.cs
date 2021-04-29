using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Common.Bus.Interfaces;
using Zero99Lotto.SRC.Common.Handlers;
using Zero99Lotto.SRC.Common.Messages.Commands.Draw;
using Zero99Lotto.SRC.Common.Messages.Events.Draw;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Services.Interfaces;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Handlers
{
    public class UpdateScheduleHandler : ICommandHandler<UpdateSchedule>
    {
        private readonly IScheduleService _scheduleService;
        private readonly IHandler _handler;
        private readonly ILogger<UpdateScheduleHandler> _logger;
        private readonly IBusPublisher _busPublisher;

        public UpdateScheduleHandler(IScheduleService scheduleService, IHandler handler,
            ILogger<UpdateScheduleHandler> logger, IBusPublisher busPublisher)
        {
            _scheduleService = scheduleService;
            _handler = handler;
            _logger = logger;
            _busPublisher = busPublisher;
        }

        public Task HandleAsync(UpdateSchedule command)
            => _handler
            .Handle(async () =>
            {
                _logger.LogInformation($"[{nameof(UpdateScheduleHandler)}] - Updating schedule {command.ScheduleId}.");
                await _scheduleService.UpdateSchedule(command.ScheduleId, command.StartTime, command.DaysOfWeek);
            })
            .OnSuccess(async () => {
                _logger.LogInformation($"[{nameof(UpdateScheduleHandler)}] - Schedule updated ({command.ScheduleId}).");

                await _busPublisher.PublishAsync(new ScheduleUpdated());
            })
            .OnCustomError(async (ex) => {
                _logger.LogError($"[{nameof(UpdateScheduleHandler)}] - Custom error occurred while updating schedule. {ex.Message}");
                await Task.FromException(ex);
            })
            .OnError(async (ex) => {
                _logger.LogError($"[{nameof(UpdateScheduleHandler)}] - Error occurred while updating schedule. Message: {ex.Message}. StackTrace: {ex.StackTrace}");
                await Task.FromException(ex);
            })
            .ExecuteAsync();

    }
}
