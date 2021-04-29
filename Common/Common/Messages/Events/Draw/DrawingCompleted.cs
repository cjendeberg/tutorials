using System;

namespace Zero99Lotto.SRC.Common.Messages.Events.Draw
{
    public class DrawingCompleted : IDrawingEvent
    {
        public Guid DrawingId { get; }
        public int[] Numbers { get; }
        public int[] ExtraNumbers { get; }

        public DrawingCompleted(Guid drawingId, int[] numbers, int[] extraNumbers)
        {
            DrawingId = drawingId;
            Numbers = numbers;
            ExtraNumbers = extraNumbers;
        }
    }
}
