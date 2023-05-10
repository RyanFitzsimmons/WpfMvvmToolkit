using System;
using System.Collections.Generic;
using System.Windows;
using WpfMvvmToolkit.Windows;

namespace WpfMvvmToolkit.Configuration
{
    public class WindowFactory : IWindowFactory
    {
        private readonly IWindowRegistry _windowRegistry;

        public WindowFactory(IWindowRegistry viewRegistry)
        {
            _windowRegistry = viewRegistry;
        }

        public Window? GetMainWindow()
        {
            return _windowRegistry.GetMainWindow();
        }

        internal IEnumerable<Window> GetWindows(IWindowViewModel viewModel)
        {
            if (_windowRegistry is not WindowRegistry wr)
            {
                throw new NotSupportedException();
            }

            return wr.GetWindows(viewModel);
        }

        public IEnumerable<TViewModel> Get<TViewModel>()
        {
            return _windowRegistry.GetExistingViewModels<TViewModel>();
        }

        public void Show<TWindowViewModel>(NavigationParameters? parameters = null, Action<WindowResult>? callback = null, IWindowViewModel? owner = null, bool isMainWindow = false) where TWindowViewModel : class, IWindowViewModel
        {
            parameters ??= new();
            var view = _windowRegistry.Get<TWindowViewModel>(parameters, callback, owner, isMainWindow);
            view.Show();
        }

        public bool? ShowDialog<TWindowViewModel>(NavigationParameters? parameters = null, Action<WindowResult>? callback = null, IWindowViewModel? owner = null, bool isMainWindow = false) where TWindowViewModel : class, IWindowViewModel
        {
            parameters ??= new();
            var view = _windowRegistry.Get<TWindowViewModel>(parameters, callback, owner, isMainWindow);
            return view.ShowDialog();
        }
    }
}
