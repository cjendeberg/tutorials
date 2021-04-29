using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zero99Lotto.SRC.Common.Messages.Commands.Draw
{
    public class CreateSchedule : ICommand
    {
        public int[] DaysOfWeek { get; }
        public TimeSpan StartTime { get; }

        [JsonConstructor]
        public CreateSchedule(int[] daysOfWeek, TimeSpan startTime)
        {
            DaysOfWeek = daysOfWeek;
            StartTime = startTime;
        }
    }
}
