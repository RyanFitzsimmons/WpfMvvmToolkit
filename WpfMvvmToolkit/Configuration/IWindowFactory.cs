using System;
using System.Collections.Generic;
using System.Windows;
using WpfMvvmToolkit.Windows;

namespace WpfMvvmToolkit.Configuration
{
    public interface IWindowFactory
    {
        Window? GetMainWindow();
        IEnumerable<TViewModel> Get<TViewModel>();
        void Show<TWindowViewModel>(NavigationParameters? parameters = null, Action<WindowResult>? callback = null, IWindowViewModel? owner = null, bool isMainWindow = false) where TWindowViewModel : class, IWindowViewModel;
        bool? ShowDialog<TWindowViewModel>(NavigationParameters? parameters = null, Action<WindowResult>? callback = null, IWindowViewModel? owner = null, bool isMainWindow = false) where TWindowViewModel : class, IWindowViewModel;
    }
}