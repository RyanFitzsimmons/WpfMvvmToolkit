using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WpfMvvmToolkit.Configuration;

namespace WpfMvvmToolkit.Microsoft;

public static class HostBuilderExtensions
{
    internal static ServiceContainer? _serviceContainer;

    public static IHostBuilder AddToolkitServiceContainer(this IHostBuilder hostBuilder)
    {
        return hostBuilder.ConfigureServices((context, services) =>
        {
            _serviceContainer = new ServiceContainer(services);
        });
    }

    internal static void ServiceContainerExists()
    {
        if (_serviceContainer == null)
        {
            throw new Exception($"{nameof(AddToolkitServiceContainer)} must be called first on {nameof(IHostBuilder)}");
        }
    }

    public static IHostBuilder ConfigureWindowFactory(this IHostBuilder hostBuilder, Action<MicrosoftWindowFactoryBuilder> builderDelegate)
    {
        hostBuilder.ConfigureServices((context, services) =>
        {
            ServiceContainerExists();
            builderDelegate(new MicrosoftWindowFactoryBuilder(_serviceContainer!));
        }); 

        return hostBuilder;
    }

    public static IWindowFactory GetWindowFactory(this IHost host)
    {
        ServiceContainerExists();
        _serviceContainer!.SetServiceProvider(host.Services);
        return host.Services.GetRequiredService<IWindowFactory>();
    }
}
