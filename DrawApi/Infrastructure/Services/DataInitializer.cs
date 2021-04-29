using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Common.Services;
using Zero99Lotto.SRC.Services.Draws.API.Core.Domain.Repositories;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Migrations;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Services
{
    public class DataInitializer : IDataInitializer
    {

        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var scheduleRepository = scope.ServiceProvider.GetRequiredService<IScheduleRepository>();
                if (scheduleRepository is IMigrateDatabase)
                    await (scheduleRepository as IMigrateDatabase).Migrate();
            }
        }
    }
}
