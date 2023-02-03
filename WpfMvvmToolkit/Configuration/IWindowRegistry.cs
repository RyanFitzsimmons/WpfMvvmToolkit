using System;
using System.Windows;
using WpfMvvmToolkit.Windows;

namespace WpfMvvmToolkit.Configuration
{
    public interface IWindowRegistry
    {
        Window Get<TWindowViewModel>(WindowParameters? parameters = null, Action<WindowParameters>? callback = null) where TWindowViewModel : IWindowViewModel;
        void Register<TWindowView, TWindowViewModel>(ScopeType scope)
            where TWindowView : Window
            where TWindowViewModel : IWindowViewModel;
    }
}