using Autofac;
using Microsoft.Extensions.Configuration;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.IoC.Modules;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.IoC
{
    public class ContainerModule : Autofac.Module
    {
        private readonly IConfiguration _configuration;

        public ContainerModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<CommandModule>();
            builder.RegisterModule<QueryModule>();
            builder.RegisterModule<RepositoryModule>();
            builder.RegisterModule<DomainEventsModule>();
            builder.RegisterModule<EventModule>();
            builder.RegisterModule<ServiceModule>();
            builder.RegisterModule(new SettingsModule(_configuration));
            builder.RegisterModule<MapperModule>();
        }
    }
}
