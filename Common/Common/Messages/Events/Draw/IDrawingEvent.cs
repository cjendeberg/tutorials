using System;

namespace Zero99Lotto.SRC.Common.Messages.Events.Draw
{
    public interface IDrawingEvent : IEvent
    {
        Guid DrawingId { get; }
    }
}
