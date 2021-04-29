using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Dispatchers;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Handlers;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.IoC.Modules
{
    public class DomainEventsModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(DomainEventsModule)
                .GetTypeInfo()
                .Assembly;

            builder.RegisterAssemblyTypes(assembly)
                   .AsClosedTypesOf(typeof(IDomainEventHandler<>))
                   .InstancePerLifetimeScope();

            builder.RegisterType<DomainEventDispatcher>()
                .As<IDomainEventDispatcher>()
                .InstancePerLifetimeScope();
        }
    }
}
