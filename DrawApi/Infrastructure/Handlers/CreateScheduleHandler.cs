using System.Threading.Tasks;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Services.Interfaces;
using Zero99Lotto.SRC.Common.Messages.Commands.Draw;
using Zero99Lotto.SRC.Common.Handlers;
using Microsoft.Extensions.Logging;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Handlers
{
    public class CreateScheduleHandler : ICommandHandler<CreateSchedule>
    {
        private readonly IHandler _handler;
        private readonly IScheduleService _scheduleService;
        private readonly ILogger<CreateSchedule> _logger;

        public CreateScheduleHandler(IHandler handler, IScheduleService scheduleService,
            ILogger<CreateSchedule> logger)
        {
            _handler = handler;
            _scheduleService = scheduleService;
            _logger = logger;
        }

        public Task HandleAsync(CreateSchedule command)
           => _handler
                .Handle(async () =>
                {
                    _logger.LogInformation($"[{nameof(CreateScheduleHandler)}] - Creating new schedule (days: {string.Join(',',command.DaysOfWeek)}, time: {command.StartTime}).");
                    await _scheduleService.CreateSchedule(command.StartTime, command.DaysOfWeek);
                })
                .OnSuccess(async () =>
                {
                    _logger.LogInformation($"[{nameof(CreateScheduleHandler)}] - Schedule created.");
                    await Task.CompletedTask;
                })
                .OnCustomError(async (ex) =>
                {
                    _logger.LogError($"[{nameof(CreateScheduleHandler)}] - Custom error occurred while creating schedule. {ex.Message}");
                    await Task.FromException(ex);

                })
                .OnError(async (ex) =>
                {
                    _logger.LogError($"[{nameof(CreateScheduleHandler)}] - Error occurred while creating schedule. Message: {ex.Message}. StackTrace: {ex.StackTrace}");
                    await Task.FromException(ex);
                })
                .ExecuteAsync();

    }
}
