using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task Show<TWindowViewModel>(NavigationParameters? parameters = null, Action<WindowResult>? callback = null, IWindowViewModel? owner = null, bool isMainWindow = false) where TWindowViewModel : class, IWindowViewModel
        {
            if (parameters == null)
            {
                parameters = new();
            }

            var view = await _windowRegistry.Get<TWindowViewModel>(parameters, callback, owner, isMainWindow).ConfigureAwait(false);
            view.Show();
        }

        public async Task<bool?> ShowDialog<TWindowViewModel>(NavigationParameters? parameters = null, Action<WindowResult>? callback = null, IWindowViewModel? owner = null, bool isMainWindow = false) where TWindowViewModel : class, IWindowViewModel
        {
            if (parameters == null)
            {
                parameters = new();
            }

            var view = await _windowRegistry.Get<TWindowViewModel>(parameters, callback, owner, isMainWindow).ConfigureAwait(false);
            return view.ShowDialog();
        }
    }
}
