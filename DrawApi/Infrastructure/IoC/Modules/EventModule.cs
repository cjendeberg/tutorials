using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Common.Handlers;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.IoC.Modules
{
    public class EventModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(EventModule)
                .GetTypeInfo()
                .Assembly;

            builder.RegisterAssemblyTypes(assembly)
                   .AsClosedTypesOf(typeof(IEventHandler<>))
                   .InstancePerLifetimeScope();
        }
    }
}
