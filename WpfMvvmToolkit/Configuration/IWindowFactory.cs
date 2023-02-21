using System;
using System.Collections.Generic;
using WpfMvvmToolkit.Windows;

namespace WpfMvvmToolkit.Configuration
{
    public interface IWindowFactory
    {
        IEnumerable<TViewModel> Get<TViewModel>();
        void Show<TWindowViewModel>(NavigationParameters? parameters = null, Action<WindowResult>? callback = null, IWindowViewModel? owner = null) where TWindowViewModel : class, IWindowViewModel;
        bool? ShowDialog<TWindowViewModel>(NavigationParameters? parameters = null, Action<WindowResult>? callback = null, IWindowViewModel? owner = null) where TWindowViewModel : class, IWindowViewModel;
    }
}