using System;
using System.Collections.Generic;
using WpfMvvmToolkit.Messaging;
using WpfMvvmToolkit.Navigation;
using WpfMvvmToolkit.Serialization;

namespace WpfMvvmToolkit.Configuration
{
    public abstract class WindowFactoryBuilder
    {
        private readonly List<Action<IServiceContainer>> _configureDelegates = new();
        private readonly List<Action<IWindowRegistry>> _registerDelegates = new();

        public WindowFactoryBuilder ConfigureServices(Action<IServiceContainer> configureDelegate)
        {
            _configureDelegates.Add(configureDelegate);
            return this;
        }

        public WindowFactoryBuilder RegisterWindows(Action<IWindowRegistry> registerDelegate)
        {
            _registerDelegates.Add(registerDelegate);
            return this;
        }

        private void ConfigureBuiltInServices(IServiceContainer container)
        {
            container.Register<IMessageService, MessageService>(ScopeType.Singleton);
            container.Register<INavigationService, NavigationService>(ScopeType.Singleton);
            container.Register<IJsonSerializationService, JsonSerializationService>(ScopeType.Singleton);
        }

        public IWindowFactory Build()
        {
            var serviceContainer = GetServiceContainer();
            serviceContainer.RegisterConstant(serviceContainer);
            serviceContainer.Register<IWindowRegistry, WindowRegistry>(ScopeType.Singleton);
            ConfigureBuiltInServices(serviceContainer);

            foreach (var configure in _configureDelegates)
            {
                configure(serviceContainer);
            }

            var windowRegistry = serviceContainer.Get<IWindowRegistry>();
            foreach (var register in _registerDelegates)
            {
                register(windowRegistry);
            }

            serviceContainer.Register<IWindowFactory, WindowFactory>(ScopeType.Singleton);
            return serviceContainer.Get<IWindowFactory>();
        }

        protected abstract IServiceContainer GetServiceContainer();
    }
}
