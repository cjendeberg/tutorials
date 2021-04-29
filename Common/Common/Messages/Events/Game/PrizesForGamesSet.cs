using System;
using System.Collections.Generic;
using System.Text;

namespace Zero99Lotto.SRC.Common.Messages.Events.Game
{
    public class PrizesForGamesSet : IEvent
    {
        public Guid DrawingId { get; }
        public int[] Numbers { get; }
        public int[] ExtraNumbers { get; }

        public PrizesForGamesSet(Guid drawingId, int[] numbers, int[] extraNumbers)
        {
            DrawingId = drawingId;
            Numbers = numbers;
            ExtraNumbers = extraNumbers;
        }
    }
}
