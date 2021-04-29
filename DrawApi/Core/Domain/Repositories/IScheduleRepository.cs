using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Services.Draws.API.Core.Domain.Entities;

namespace Zero99Lotto.SRC.Services.Draws.API.Core.Domain.Repositories
{
    public interface IScheduleRepository : IRepository
    {
        Task<Schedule> GetAsync(Guid id);
        Task<Schedule> GetAsync();
        Task<Schedule> GetByDrawingAsync(Guid drawingId);
        Task AddAsync(Schedule schedule);
        Task UpdateAsync(Schedule schedule);
        Task<IEnumerable<Schedule>> ListSchedules();
    }
}
