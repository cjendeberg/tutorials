using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Migrations
{
    public interface IMigrateDatabase
    {
        Task Migrate();
    }
}
