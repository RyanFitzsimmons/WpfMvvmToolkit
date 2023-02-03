using System;
using System.Collections.Generic;

namespace WpfMvvmToolkit.Configuration
{
    public abstract class WindowFactoryBuilder
    {
        private List<Action<IServiceContainer>> _configureDelegates = new();
        private List<Action<IWindowRegistry>> _registerDelegates = new();

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

        public IWindowFactory Build()
        {
            var serviceContainer = GetServiceContainer();
            serviceContainer.RegisterConstant(serviceContainer);
            serviceContainer.Register<IWindowRegistry, WindowRegistry>(ScopeType.Singleton);

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
