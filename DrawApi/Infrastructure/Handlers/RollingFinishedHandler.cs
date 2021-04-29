using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Common.Bus.Interfaces;
using Zero99Lotto.SRC.Common.Handlers;
using Zero99Lotto.SRC.Common.Messages.Events.Draw;
using Zero99Lotto.SRC.Common.Messages.Events.Rolling;
using Zero99Lotto.SRC.Services.Draws.API.Core.Domain.Repositories;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Dispatchers;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Services.Interfaces;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Handlers
{
    public class RollingFinishedHandler : IEventHandler<RollingFinished>
    {
        private readonly IHandler _handler;
        private readonly IDrawingService _drawingService;
        private readonly IDrawingRepository _drawingRepository;
        private readonly IBusPublisher _busPublisher;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        private readonly ILogger<RollingFinished> _logger;

        public RollingFinishedHandler(IHandler handler, IDrawingService drawingService,
            IDrawingRepository drawingRepository, IBusPublisher busPublisher,
            IDomainEventDispatcher domainEventDispatcher, ILogger<RollingFinished> logger)
        {
            _handler = handler;
            _drawingService = drawingService;
            _drawingRepository = drawingRepository;
            _busPublisher = busPublisher;
            _domainEventDispatcher = domainEventDispatcher;
            _logger = logger;
        }

        public Task HandleAsync(RollingFinished @event)
            =>
            _handler
                .Handle(async () =>
                {
                    _logger.LogInformation($"[{nameof(RollingFinishedHandler)}] - Updating drawing ({@event.DrawingId}) after rolling finished.");
                    await _drawingService.CompleteDrawing(@event.DrawingId, @event.Numbers, @event.ExtraNumbers);
                })
                .OnSuccess(async () =>
                {
                    _logger.LogInformation($"[{nameof(RollingFinishedHandler)}] - Drawing completed ({@event.DrawingId}).");
                    await _busPublisher.PublishAsync(new DrawingCompleted(@event.DrawingId,
                        @event.Numbers, @event.ExtraNumbers));
                    //var drawing = await _drawingRepository.GetAsync(@event.DrawingId);
                    //await _domainEventDispatcher.DispatchAsync(drawing.Events.ToArray());
                })
                .OnCustomError(async (ex) =>
                {
                    _logger.LogError($"[{nameof(RollingFinishedHandler)}] - Custom error occurred while updating drawing after rolling finished. {ex.Message}");
                    await Task.FromException(ex);
                })
                .OnError(async (ex) =>
                {
                    _logger.LogError($"[{nameof(RollingFinishedHandler)}] - Error occurred while updating drawing after rolling finished. Message: {ex.Message}. StackTrace: {ex.StackTrace}");
                    await Task.FromException(ex);
                })
                .ExecuteAsync();
    }
}
