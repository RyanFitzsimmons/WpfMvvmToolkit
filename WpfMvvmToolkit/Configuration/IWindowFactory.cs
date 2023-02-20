using System;
using WpfMvvmToolkit.Windows;

namespace WpfMvvmToolkit.Configuration
{
    public interface IWindowFactory
    {
        void Show<TWindowViewModel>(NavigationParameters? parameters = null, Action<IWindowResult>? callback = null) where TWindowViewModel : class, IWindowViewModel;
        bool? ShowDialog<TWindowViewModel>(NavigationParameters? parameters = null, Action<IWindowResult>? callback = null) where TWindowViewModel : class, IWindowViewModel;
    }
}