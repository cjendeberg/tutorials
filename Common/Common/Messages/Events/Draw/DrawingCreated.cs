using System;

namespace Zero99Lotto.SRC.Common.Messages.Events.Draw
{
    public class DrawingCreated: IDrawingEvent
    {
        public Guid DrawingId { get; }
        public DateTime StartDate { get; }

        public DrawingCreated(Guid drawingId, DateTime startDate)
        {
            DrawingId = drawingId;
            StartDate = startDate;
        }
    }
}
