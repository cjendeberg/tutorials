using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Services.Draws.API.Core.Domain.Entities;
using Zero99Lotto.SRC.Services.Draws.API.Core.Domain.Repositories;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Dispatchers;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Services.Interfaces;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        private readonly IScheduleRepository _scheduleRespository;

        public ScheduleService(IDomainEventDispatcher domainEventDispatcher,
            IScheduleRepository scheduleRepository)
        {
            _domainEventDispatcher = domainEventDispatcher;
            _scheduleRespository = scheduleRepository;
        }

        public async Task CreateSchedule(TimeSpan startTime, int[] daysOfWeek)
        {
            var schedule = Schedule.CreateSchedule(startTime, daysOfWeek);
            await _scheduleRespository.AddAsync(schedule).ConfigureAwait(false);
            await _domainEventDispatcher.DispatchAsync(schedule.Events.ToArray()).ConfigureAwait(false);
        }

        public async Task UpdateSchedule(Guid scheduleId, TimeSpan startTime, int[] daysOfWeek)
        {
            var schedule = await _scheduleRespository.GetAsync(scheduleId).ConfigureAwait(false);

            if (schedule == null)
                throw new ServiceException($"Schedule with id {scheduleId} does not exist!");

            if (!schedule.StartTime.Equals(startTime))
                schedule.UpdateTime(startTime);
 
            if (!schedule.Days.Select(x=>x.DayOfWeek).SequenceEqual(daysOfWeek))
                schedule.UpdateDays(daysOfWeek);

            await _scheduleRespository.UpdateAsync(schedule).ConfigureAwait(false);
            await _domainEventDispatcher.DispatchAsync(schedule.Events.ToArray()).ConfigureAwait(false);
        }
    }
}
