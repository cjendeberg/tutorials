using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Zero99Lotto.SRC.Services.Draws.API.Core.Domain.Entities;
using Zero99Lotto.SRC.Services.Draws.API.Core.Domain.Repositories;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.EF;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Migrations;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Repositories
{
    public class ScheduleRepository : IScheduleRepository, IMigrateDatabase
    {
        private readonly DrawContext _context;

        public ScheduleRepository(DrawContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Schedule schedule)
        {
            await _context.Schedule.AddAsync(schedule).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public Task<Schedule> GetAsync(Guid id) 
            =>  _context.Schedule.Include(x => x.Days).Include(x => x.Drawings)
                                 .SingleOrDefaultAsync(x => x.Id == id);

        public Task<Schedule> GetByDrawingAsync(Guid drawingId)
            => _context.Schedule.Include(x=>x.Days)
                                .SingleOrDefaultAsync(x => x.Drawings.Any(y => y.Id == drawingId));

        public async Task UpdateAsync(Schedule schedule)
        {
            _context.Schedule.Update(schedule);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
        public Task Migrate()
        {
            // Migrate databases only in Development
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Contains("Development", System.StringComparison.OrdinalIgnoreCase))
            {
                return _context.Database.MigrateAsync();
            }
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Schedule>> ListSchedules()
            => await _context.Schedule.Include(x=>x.Days).Include(x=>x.Drawings).ToListAsync().ConfigureAwait(false);

        public Task<Schedule> GetAsync()
            => _context.Schedule.Include(x => x.Days).Include(x => x.Drawings).SingleOrDefaultAsync();

    }
}
