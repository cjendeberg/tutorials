using Autofac;
using Autofac.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace t7
{
  class Program
  {
    static void Main(string[] args)
    {
      var builder = new ContainerBuilder();
      builder.RegisterModule(new ContainerModule());
      var container = builder.Build();
      IServiceProvider sp = new AutofacServiceProvider(container);
    }
  }
}
