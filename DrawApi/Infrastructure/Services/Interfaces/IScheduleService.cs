using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Common.Services;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Services.Interfaces
{
    public interface IScheduleService : IService
    {
        Task CreateSchedule(TimeSpan startTime, int[] daysOfWeek);
        Task UpdateSchedule(Guid scheduleId, TimeSpan startTime, int[] daysOfWeek);
    }
}
