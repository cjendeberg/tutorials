using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Common.Bus.Interfaces;
using Zero99Lotto.SRC.Common.Handlers;
using Zero99Lotto.SRC.Common.Messages.Commands.Draw;
using Zero99Lotto.SRC.Common.Messages.Events.Draw;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Services.Interfaces;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Handlers
{
    public class UpdateDrawingHandler : ICommandHandler<UpdateDrawing>
    {
        private readonly IHandler _handler;
        private readonly IDrawingService _drawingService;
        private readonly ILogger<UpdateDrawingHandler> _logger;
        private readonly IBusPublisher _busPublisher;

        public UpdateDrawingHandler(IHandler handler, IDrawingService drawingService,
            IBusPublisher busPublisher, ILogger<UpdateDrawingHandler> logger)
        {
            _handler = handler;
            _drawingService = drawingService;
            _logger = logger;
            _busPublisher = busPublisher;
        }

        public Task HandleAsync(UpdateDrawing command)
            => _handler.Handle(async () =>
            {
                _logger.LogInformation($"[{nameof(UpdateDrawingHandler)}] - Updating numbers of drawing {command.DrawingId}");
                await _drawingService.UpdateDrawingNumbers(command.DrawingId, command.Numbers, command.ExtraNumbers);
            })
            .OnSuccess(async () =>
            {
                var drawing = _drawingService.GetDrawing(command.DrawingId);

                _logger.LogInformation($"[{nameof(UpdateDrawingHandler)}] - Drawing numbers updated ({drawing.Id}).");

                await _busPublisher.PublishAsync(new DrawingUpdated(command.DrawingId, (await drawing).StartDate, command.Numbers, command.ExtraNumbers));
            })
            .OnCustomError(async (ex) =>
            {
                _logger.LogError($"[{nameof(UpdateDrawingHandler)}] - Custom error occurred while updating numbers of drawing. {ex.Message}");
                await Task.FromException(ex);
            })
            .OnError(async (ex) =>
            {
                _logger.LogError($"[{nameof(UpdateDrawingHandler)}] - Error occurred while updating numbers of drawing. Message: {ex.Message}. StackTrace: {ex.StackTrace}");
                await Task.FromException(ex);
            })
            .ExecuteAsync();


    }
}
