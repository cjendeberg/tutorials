using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Common.Services;

namespace Zero99Lotto.SRC.Common.Extensions
{
    public static class WebHostExtensions
    {
        public static IWebHost InitializeDatabase(this IWebHost host, IDataInitializer dataInitializer)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<IDataInitializer>>();

                try
                {
                    logger.LogInformation($"Migrating the database...");

                    var retry = Policy.Handle<SqlException>()
                         .WaitAndRetry(new TimeSpan[]
                         {
                             TimeSpan.FromSeconds(3),
                             TimeSpan.FromSeconds(5),
                             TimeSpan.FromSeconds(8),
                             TimeSpan.FromSeconds(13),
                         });

                    retry.Execute(()=>dataInitializer.SeedAsync(scope.ServiceProvider).Wait());

                    logger.LogInformation($"Database migrated!");

                }
                catch(Exception ex)
                {
                    logger.LogError(ex, $"An error occurred while migrating the database.");
                }
               
            }

            return host;
        }
    }
}
