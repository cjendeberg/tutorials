using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zero99Lotto.SRC.Common.Messages.Commands.Draw
{
    public class UpdateDrawing : ICommand
    {
        public Guid DrawingId { get; }
        public int[] Numbers { get; }
        public int[] ExtraNumbers { get; }

        [JsonConstructor]
        public UpdateDrawing(Guid drawingId, int[] numbers, int[] extraNumbers)
        {
            DrawingId = drawingId;
            Numbers = numbers;
            ExtraNumbers = extraNumbers;
        }
    }
}
