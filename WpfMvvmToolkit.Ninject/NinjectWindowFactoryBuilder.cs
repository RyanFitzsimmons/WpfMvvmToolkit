using WpfMvvmToolkit.Configuration;

namespace WpfMvvmToolkit.Ninject
{
    public class NinjectWindowFactoryBuilder : WindowFactoryBuilder
    {
        protected override IServiceContainer GetServiceContainer()
        {
            return new ServiceContainer();
        }
    }
}
