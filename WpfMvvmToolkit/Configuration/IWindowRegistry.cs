using System;
using System.Windows;
using WpfMvvmToolkit.Windows;

namespace WpfMvvmToolkit.Configuration
{
    public interface IWindowRegistry
    {
        IWindowView Get<TWindowViewModel>(NavigationParameters parameters, Action<IWindowResult>? callback = null) where TWindowViewModel : IWindowViewModel;
        
        void Register<TWindowView, TWindowViewModel>(ScopeType scope)
            where TWindowView : Window, IWindowView
            where TWindowViewModel : IWindowViewModel;

        void Register(Type viewType, Type viewModelType, ScopeType scope);
    }
}