using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace t1
{
    public class UpdateSchedule : ICommand
    {
        public Guid ScheduleId { get; }
        public int[] DaysOfWeek { get; }
        public TimeSpan StartTime { get; }

        [JsonConstructor]
        public UpdateSchedule(Guid scheduleId, int[] daysOfWeek, TimeSpan startTime)
        {
            DaysOfWeek = daysOfWeek;
            StartTime = startTime;
            ScheduleId = scheduleId;
        }
    }
}
