using System;
using System.Collections.Generic;
using System.Text;

namespace Zero99Lotto.SRC.Common.Messages.Events.Rolling
{
    public class RollingFinished : IEvent
    {
        public Guid DrawingId { get; }
        public int[] Numbers { get; }
        public int[] ExtraNumbers { get; }

        public RollingFinished(Guid drawingId, int[] numbers, int[] extraNumbers)
        {
            DrawingId = drawingId;
            Numbers = numbers;
            ExtraNumbers = extraNumbers;
        }
    }
}
