using WpfMvvmToolkit.Configuration;

namespace WpfMvvmToolkit.Microsoft;

public class MicrosoftWindowFactoryBuilder : WindowFactoryBuilder
{
    private ServiceContainer _serviceContainer;

    public MicrosoftWindowFactoryBuilder(ServiceContainer serviceContainer)
    {
        _serviceContainer = serviceContainer;
    }

    protected override IServiceContainer GetServiceContainer()
    {
        return _serviceContainer;
    }
}