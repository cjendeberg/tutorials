using System;
using System.Collections.Generic;
using System.Text;

namespace Zero99Lotto.SRC.Common.Messages.Events.Rolling
{
    public class RollingFailed : IEvent
    {
        public Guid DrawingId { get; }

        public RollingFailed(Guid drawingId)
            => DrawingId = drawingId;
    }
}
