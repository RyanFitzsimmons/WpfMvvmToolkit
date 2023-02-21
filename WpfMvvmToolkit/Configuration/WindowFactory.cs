using System;
using System.Collections.Generic;
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

        public IEnumerable<TViewModel> Get<TViewModel>()
        {
            return _windowRegistry.GetExistingViewModels<TViewModel>();
        }

        public void Show<TWindowViewModel>(NavigationParameters? parameters = null, Action<WindowResult>? callback = null, IWindowViewModel? owner = null) where TWindowViewModel : class, IWindowViewModel
        {
            if (parameters == null)
            {
                parameters = new();
            }

            var view = _windowRegistry.Get<TWindowViewModel>(parameters, callback, owner);
            view.Show();
        }

        public bool? ShowDialog<TWindowViewModel>(NavigationParameters? parameters = null, Action<WindowResult>? callback = null, IWindowViewModel? owner = null) where TWindowViewModel : class, IWindowViewModel
        {
            if (parameters == null)
            {
                parameters = new();
            }

            var view = _windowRegistry.Get<TWindowViewModel>(parameters, callback, owner);
            return view.ShowDialog();
        }
    }
}
