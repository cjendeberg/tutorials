using Autofac;

namespace t7
{
    public class ContainerModule : Autofac.Module
    {

        public ContainerModule()
        {
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<Class1>();
        }
    }
}
