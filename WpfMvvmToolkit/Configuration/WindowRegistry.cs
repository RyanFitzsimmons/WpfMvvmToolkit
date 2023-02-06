using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using WpfMvvmToolkit.Windows;

namespace WpfMvvmToolkit.Configuration
{
    internal class WindowRegistry : IWindowRegistry
    {
        private readonly Dictionary<Type, WindowRegistration> _viewRegistrationLookup = new();
        private readonly Dictionary<Type, WindowRegistration> _viewModelRegistrationLookup = new();
        private readonly Dictionary<IWindowViewModel, IWindowView> _activeWindows = new();
        private readonly Dictionary<IWindowViewModel, Action<IWindowResult>?> _callbacks = new();
        private readonly IServiceContainer _serviceContainer;

        public WindowRegistry(IServiceContainer serviceContainer)
        {
            _serviceContainer = serviceContainer;
        }

        public void Register<TWindowView, TWindowViewModel>(ScopeType scope)
            where TWindowView : Window, IWindowView
            where TWindowViewModel : IWindowViewModel
        {
            if (_viewRegistrationLookup.ContainsKey(typeof(TWindowView)))
            {
                throw new ArgumentException($"The view {typeof(TWindowView).Name} has already been registered.");
            }

            _serviceContainer.RegisterToSelf(typeof(TWindowView), scope);
            _serviceContainer.RegisterToSelf(typeof(TWindowViewModel), scope);

            var windowRegistration = new WindowRegistration(typeof(TWindowView), typeof(TWindowViewModel), scope);
            _viewRegistrationLookup.Add(typeof(TWindowView), windowRegistration);
            _viewModelRegistrationLookup.Add(typeof(TWindowViewModel), windowRegistration);
        }

        public IWindowView Get<TWindowViewModel>(WindowParameters? parameters = null, Action<IWindowResult>? callback = null)
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

        private void ViewModel_Close(IWindowResult result)
        {
            var window = _activeWindows[result.ViewModel];
            window.Result = result;
            window.Close();
        }
    }
}
