using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Autofac;

namespace t7
{
  public class Class1 : Autofac.Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      var assembly = typeof(Class1)
          .GetTypeInfo()
          .Assembly;

      builder.RegisterAssemblyTypes(assembly)
             .AsClosedTypesOf(typeof(ICommandHandler<>))
             .InstancePerLifetimeScope();
    }
  }
}
