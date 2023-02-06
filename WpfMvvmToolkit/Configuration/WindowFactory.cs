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

        public void Show<TWindowViewModel>(WindowParameters? parameters = null, Action<IWindowResult>? callback = null) where TWindowViewModel : class, IWindowViewModel
        {
            var view = _windowRegistry.Get<TWindowViewModel>(parameters, callback);
            view.Show();
        }

        public bool? ShowDialog<TWindowViewModel>(WindowParameters? parameters = null, Action<IWindowResult>? callback = null) where TWindowViewModel : class, IWindowViewModel
        {
            var view = _windowRegistry.Get<TWindowViewModel>(parameters, callback);
            return view.ShowDialog();
        }
    }
}
