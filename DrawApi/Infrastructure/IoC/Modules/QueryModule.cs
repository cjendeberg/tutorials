using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Common.Dispatchers;
using Zero99Lotto.SRC.Common.Handlers;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Dispatchers;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Handlers;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.IoC.Modules
{
    public class QueryModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(QueryModule)
                .GetTypeInfo()
                .Assembly;

            builder.RegisterAssemblyTypes(assembly)
                   .AsClosedTypesOf(typeof(IQueryHandler<,>))
                   .InstancePerLifetimeScope();

            builder.RegisterType<QueryDispatcher>()
                .As<IQueryDispatcher>()
                .InstancePerLifetimeScope();
        }
    }
}
