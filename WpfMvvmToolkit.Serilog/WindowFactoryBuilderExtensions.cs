using Serilog;
using Serilog.Events;
using WpfMvvmToolkit.Configuration;

namespace WpfMvvmToolkit.Serilog
{
    public static class WindowFactoryBuilderExtensions
    {
        public static void AddSerilog(this IServiceContainer serviceContainer, string logsDirectoryPath)
        {
            DirectoryInfo directory = new(logsDirectoryPath);
            if (!directory.Exists)
            {
                directory.Create();
            }

            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Is(LogEventLevel.Debug)
                .WriteTo.File($@"{directory.FullName}\log.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Debug()
            .CreateLogger();

            serviceContainer.RegisterConstant<ILogger>(Log.Logger);
        }

        public static WindowFactoryBuilder AddSerilog(this WindowFactoryBuilder windowFactoryBuilder, Action<IServiceContainer> configureDelegate)
        {
            windowFactoryBuilder.ConfigureServices(configureDelegate);

            return windowFactoryBuilder;
        }
    }
}