using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zero99Lotto.SRC.Common.Messages.Events.Draw
{
    public class ScheduleUpdated : IEvent
    {
        [JsonConstructor]
        public ScheduleUpdated()
        {

        }
    }
}
