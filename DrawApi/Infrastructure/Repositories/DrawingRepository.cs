using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Services.Draws.API.Core.Domain.Entities;
using Zero99Lotto.SRC.Services.Draws.API.Core.Domain.Repositories;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.EF;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Migrations;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Repositories
{
    public class DrawingRepository : IDrawingRepository, IMigrateDatabase
    {
        private readonly DrawContext _context;

        public DrawingRepository(DrawContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Drawing drawing)
        {
            _context.Drawing.AddAsync(drawing);
            await _context.SaveChangesAsync();
        }

        public Task<Drawing> GetAsync(Guid id) => _context.Drawing.Include(x=>x.Numbers).SingleOrDefaultAsync(x => x.Id == id);

        public async Task UpdateAsync(Drawing drawing)
        {
            _context.Drawing.Update(drawing);
            await _context.SaveChangesAsync();
        }

        public async Task Migrate()
        {
            // Migrate databases only in Development
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Contains("Development", System.StringComparison.OrdinalIgnoreCase))
            {
                await _context.Database.MigrateAsync();
            }
        }

        public Task<Drawing> GetActiveAsync()
            => _context.Drawing.Include(x=>x.Numbers).SingleOrDefaultAsync(x => x.Active);
    }
}
