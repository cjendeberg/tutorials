using System;

namespace Zero99Lotto.SRC.Common.Messages.Events.Draw
{
    public class DrawingUpdated : IDrawingEvent
    {
        public Guid DrawingId { get; }
        public DateTime StartDate { get; }
        public int[] Numbers { get; }
        public int[] ExtraNumbers { get; }

        public DrawingUpdated(Guid drawingId, DateTime startDate, int[] numbers, int[]extraNumbers)
        {
            DrawingId = drawingId;
            StartDate = startDate;
            Numbers = numbers;
            ExtraNumbers = extraNumbers;
        }
    }
}
