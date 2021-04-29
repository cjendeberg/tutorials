using Autofac;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Common.Extensions;
using Zero99Lotto.SRC.Common.Settings;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Extensions;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Setings;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.IoC.Modules
{
    public class SettingsModule : Autofac.Module
    {
        private readonly IConfiguration _configuration;

        public SettingsModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_configuration.GetSettings<SqlSettings>())
                   .SingleInstance();
            builder.RegisterInstance(_configuration.GetSettings<DrawingSettings>())
                    .SingleInstance();
        }
    }
}
