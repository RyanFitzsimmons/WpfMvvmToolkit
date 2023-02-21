using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using WpfMvvmToolkit.Windows;

namespace WpfMvvmToolkit.Configuration
{
    internal class WindowRegistry : IWindowRegistry
    {
        private readonly Dictionary<Type, WindowRegistration> _viewRegistrationLookup = new();
        private readonly Dictionary<Type, WindowRegistration> _viewModelRegistrationLookup = new();
        private readonly Dictionary<IWindowViewModel, IWindowView> _activeWindows = new();
        private readonly Dictionary<IWindowViewModel, Action<WindowResult>?> _callbacks = new();
        private readonly IServiceContainer _serviceContainer;

        public WindowRegistry(IServiceContainer serviceContainer)
        {
            _serviceContainer = serviceContainer;
        }

        internal IEnumerable<Window> GetWindows(IWindowViewModel viewModel)
        {
            return _activeWindows.Where(x => x.Key == viewModel).Select(x => x.Value).Cast<Window>();
        }

        public void Register<TWindowView, TWindowViewModel>(ScopeType scope)
            where TWindowView : Window, IWindowView
            where TWindowViewModel : IWindowViewModel
        {
            Register(typeof(TWindowView), typeof(TWindowViewModel), scope);
        }

        public void Register(Type view, Type viewModel, ScopeType scope)
        {
            if (_viewRegistrationLookup.ContainsKey(view))
            {
                throw new ArgumentException($"The view {view.Name} has already been registered.");
            }

            _serviceContainer.RegisterToSelf(view, ScopeType.Transient);
            _serviceContainer.RegisterToSelf(viewModel, scope);

            var windowRegistration = new WindowRegistration(view, viewModel, scope);
            _viewRegistrationLookup.Add(view, windowRegistration);
            _viewModelRegistrationLookup.Add(viewModel, windowRegistration);
        }

        public IEnumerable<TViewModel> GetExistingViewModels<TViewModel>()
        {
            return _activeWindows.Keys.Where(x => x is TViewModel).Cast<TViewModel>();
        }

        public IWindowView Get<TWindowViewModel>(NavigationParameters parameters, Action<WindowResult>? callback = null, IWindowViewModel? owner = null)
            where TWindowViewModel : IWindowViewModel
        {
            if (!_viewModelRegistrationLookup.ContainsKey(typeof(TWindowViewModel)))
            {
                throw new ArgumentException($"The view model {typeof(TWindowViewModel).Name} was never registered.");
            }

            var registration = _viewModelRegistrationLookup[typeof(TWindowViewModel)];

            var viewModel = _serviceContainer.Get<TWindowViewModel>();
            viewModel.OnOpen(parameters);
            viewModel.Close += ViewModel_Close;

            var view = (IWindowView)_serviceContainer.Get(registration.ViewType);

            if (owner != null && _activeWindows.ContainsKey(owner))
            {
                view.Owner = (Window)_activeWindows[owner];
            }

            view.DataContext = viewModel;
            view.Loaded += View_Loaded;
            view.Unloaded += View_Unloaded;
            view.Closing += View_Closing;
            view.Closed += View_Closed;

            _activeWindows.Add(viewModel, view);
            _callbacks.Add(viewModel, callback);

            return view;
        }

        private async void View_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is not Window window)
            {
                throw new Exception($"The sender is not the window");
            }

            if (window.DataContext is not IWindowViewModel viewModel)
            {
                throw new Exception($"The data context is not a window view model");
            }

            await viewModel.Load();
        }

        private void View_Unloaded(object sender, RoutedEventArgs e)
        {
            if (sender is not Window window)
            {
                throw new Exception($"The sender is not the window");
            }

            if (window.DataContext is not IWindowViewModel viewModel)
            {
                throw new Exception($"The data context is not a window view model");
            }

            viewModel.Unload();
        }

        private void View_Closing(object? sender, CancelEventArgs e)
        {
            if (sender is not Window window)
            {
                throw new Exception($"The sender is not the window");
            }

            if (window.DataContext is not IWindowViewModel viewModel)
            {
                throw new Exception($"The data context is not a window view model");
            }

            viewModel.OnClosing(e);
        }

        private void View_Closed(object? sender, EventArgs e)
        {
            if (sender is not IWindowView window)
            {
                throw new Exception($"The sender is not the window");
            }

            if (window.DataContext is not IWindowViewModel viewModel)
            {
                throw new Exception($"The data context is not a window view model");
            }

            window.Loaded -= View_Loaded;
            window.Unloaded -= View_Unloaded;
            window.Closing -= View_Closing;
            window.Closed -= View_Closed;

            viewModel.OnClose();
            _callbacks[viewModel]?.Invoke(window.Result ?? new WindowResult(viewModel, new(), ""));

            viewModel.Close -= ViewModel_Close;
            _callbacks.Remove(viewModel);
            _activeWindows.Remove(viewModel);
        }

        private void ViewModel_Close(WindowResult result)
        {
            var window = _activeWindows[result.ViewModel];
            window.Result = result;
            window.Close();
        }
    }
}
