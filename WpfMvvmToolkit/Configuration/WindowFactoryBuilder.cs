using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.Formats.Asn1;
using System.Linq;
using System.Reflection;
using System.Windows.Forms.Design;
using WpfMvvmToolkit.Dialogs;
using WpfMvvmToolkit.Exceptions;
using WpfMvvmToolkit.Messaging;
using WpfMvvmToolkit.Navigation;
using WpfMvvmToolkit.Serialization;
using WpfMvvmToolkit.Windows;

namespace WpfMvvmToolkit.Configuration
{
    public abstract class WindowFactoryBuilder
    {
        private readonly List<Action<IServiceContainer>> _configureDelegates = new();
        private readonly List<Action<IWindowRegistry>> _registerDelegates = new();
        private readonly List<Assembly> _serviceAssemblies = new();
        private readonly List<Assembly> _viewModelsAssemblies = new();
        private readonly List<Assembly> _windowAssemblies = new();
        private Func<Type, Type, ScopeType>? _onRegisterSetScope;
        private Func<Type, Type, bool>? _onRegisterIgnore;

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

        private void RegisterBuiltInServices(IServiceContainer container)
        {
            Register(container, typeof(IMessageService), typeof(MessageService));
            Register(container, typeof(INavigationService), typeof(NavigationService));
            Register(container, typeof(IJsonSerializationService), typeof(JsonSerializationService));
            Register(container, typeof(IWindowsDialogService), typeof(WindowsDialogService));
        }

        public IWindowFactory Build()
        {
            var serviceContainer = GetServiceContainer();
            serviceContainer.RegisterConstant(serviceContainer);
            serviceContainer.Register<IWindowRegistry, WindowRegistry>(ScopeType.Singleton);
            RegisterBuiltInServices(serviceContainer);
            RegisterServicesFromAssemblies(serviceContainer);
            RegisterViewModelsFromAssemblies(serviceContainer);

            foreach (var configure in _configureDelegates)
            {
                configure(serviceContainer);
            }

            var windowRegistry = serviceContainer.Get<IWindowRegistry>();
            RegisterWindowsFromAssemblies(windowRegistry);

            foreach (var register in _registerDelegates)
            {
                register(windowRegistry);
            }

            serviceContainer.Register<IWindowFactory, WindowFactory>(ScopeType.Singleton);
            return serviceContainer.Get<IWindowFactory>();
        }

        protected abstract IServiceContainer GetServiceContainer();

        public WindowFactoryBuilder AddWindowAssembly(Assembly assembly)
        {
            _windowAssemblies.Add(assembly);
            return this;
        }

        public WindowFactoryBuilder AddServiceAssembly(Assembly assembly)
        {
            _serviceAssemblies.Add(assembly);
            return this;
        }

        public WindowFactoryBuilder AddViewModelsAssembly(Assembly assembly)
        {
            _viewModelsAssemblies.Add(assembly);
            return this;
        }

        private void RegisterWindowsFromAssemblies(IWindowRegistry windowRegistry)
        {
            foreach (var assembly in _windowAssemblies)
            {
                foreach (var window in GetWindowsFrom(assembly))
                {
                    Register(windowRegistry, window.View, window.ViewModel);
                }
            }
        }

        private void RegisterServicesFromAssemblies(IServiceContainer serviceContainer)
        {
            foreach (var assembly in _serviceAssemblies)
            {
                foreach (var service in GetServicesFrom(assembly))
                {
                    Register(serviceContainer, service.Interface, service.Implementation);
                }
            }
        }

        private void RegisterViewModelsFromAssemblies(IServiceContainer serviceContainer)
        {
            foreach (var assembly in _viewModelsAssemblies)
            {
                foreach (var service in GetViewModelsFrom(assembly))
                {
                    Register(serviceContainer, service.Interface, service.Implementation);
                }
            }
        }

        private IEnumerable<(Type Interface, Type Implementation)> GetViewModelsFrom(Assembly assembly)
        {
            var allTypes = assembly.GetTypes();
            var classes = allTypes.Where(x => x.IsClass && !x.IsAbstract && x.Name.EndsWith("ViewModel"));

            foreach (var c in classes)
            {
                var i = allTypes.SingleOrDefault(x => x.IsInterface && x.Name == $"I{c.Name}");

                if (i == null)
                {
                    continue;
                }

                if (!i.IsAssignableFrom(c))
                {
                    throw new IncompatibleTypeRegistrationException(i, c);
                }

                yield return (i, c);
            }
        }

        private IEnumerable<(Type Interface, Type Implementation)> GetServicesFrom(Assembly assembly)
        {
            var allTypes = assembly.GetTypes();
            var classes = allTypes.Where(x => x.IsClass && !x.IsAbstract && x.Name.EndsWith("Service"));

            foreach (var c in classes)
            {
                var i = allTypes.SingleOrDefault(x => x.IsInterface && x.Name == $"I{c.Name}");

                if (i == null)
                {
                    continue;
                }

                if (!i.IsAssignableFrom(c))
                {
                    throw new IncompatibleTypeRegistrationException(i, c);
                }

                yield return (i, c);
            }
        }

        private IEnumerable<(Type View, Type ViewModel)> GetWindowsFrom(Assembly assembly)
        {
            var allTypes = assembly.GetTypes();
            var windowClasses = allTypes.Where(x => x.IsClass && !x.IsAbstract && x.Name.EndsWith("Window"));

            foreach (var window in windowClasses)
            {
                var viewModel = allTypes.SingleOrDefault(x => x.IsClass && !x.IsAbstract && x.Name == $"{window.Name}ViewModel");

                if (viewModel == null)
                {
                    continue;
                }

                yield return (window, viewModel);
            }
        }

        public WindowFactoryBuilder OnRegisterSetScope(Func<Type, Type, ScopeType> onRegisterSetScope)
        {
            _onRegisterSetScope = onRegisterSetScope;
            return this;
        }

        public WindowFactoryBuilder OnRegisterIgnore(Func<Type, Type, bool> onRegisterIgnore)
        {
            _onRegisterIgnore = onRegisterIgnore;
            return this;
        }

        private void Register(IServiceContainer container, Type serviceInterface, Type serviceImplementation)
        {
            var ignore = _onRegisterIgnore?.Invoke(serviceInterface, serviceImplementation) ?? false;

            if (ignore)
            {
                return;
            }

            var scope = _onRegisterSetScope?.Invoke(serviceInterface, serviceImplementation) ?? ScopeType.Singleton;
            container.Register(serviceInterface, serviceImplementation, scope);
        }

        private void Register(IWindowRegistry windowRegistry, Type view, Type viewModel)
        {
            var ignore = _onRegisterIgnore?.Invoke(view, viewModel) ?? false;

            if (ignore)
            {
                return;
            }

            var scope = _onRegisterSetScope?.Invoke(view, viewModel) ?? ScopeType.Singleton;
            windowRegistry.Register(view, viewModel, scope);
        }

    }
}
