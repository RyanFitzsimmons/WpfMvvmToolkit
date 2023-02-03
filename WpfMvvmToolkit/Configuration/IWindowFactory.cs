using System;
using WpfMvvmToolkit.Windows;

namespace WpfMvvmToolkit.Configuration
{
    public interface IWindowFactory
    {
        void Show<TWindowViewModel>(WindowParameters? parameters = null, Action<WindowParameters>? callback = null) where TWindowViewModel : class, IWindowViewModel;
        bool? ShowDialog<TWindowViewModel>(WindowParameters? parameters = null, Action<WindowParameters>? callback = null) where TWindowViewModel : class, IWindowViewModel;
    }
}