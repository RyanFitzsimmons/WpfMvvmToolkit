using System;
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

        public void Show<TWindowViewModel>(NavigationParameters? parameters = null, Action<IWindowResult>? callback = null) where TWindowViewModel : class, IWindowViewModel
        {
            if (parameters == null)
            {
                parameters = new();
            }

            var view = _windowRegistry.Get<TWindowViewModel>(parameters, callback);
            view.Show();
        }

        public bool? ShowDialog<TWindowViewModel>(NavigationParameters? parameters = null, Action<IWindowResult>? callback = null) where TWindowViewModel : class, IWindowViewModel
        {
            if (parameters == null)
            {
                parameters = new();
            }

            var view = _windowRegistry.Get<TWindowViewModel>(parameters, callback);
            return view.ShowDialog();
        }
    }
}
