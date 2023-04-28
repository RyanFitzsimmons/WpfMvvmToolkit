using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using WpfMvvmToolkit.Windows;

namespace WpfMvvmToolkit.Configuration
{
    public interface IWindowRegistry
    {
        Window? GetMainWindow();

        IEnumerable<TViewModel> GetExistingViewModels<TViewModel>();

        Task<IWindowView> Get<TWindowViewModel>(NavigationParameters parameters, Action<WindowResult>? callback = null, IWindowViewModel? owner = null, bool isMainWindow = false) where TWindowViewModel : IWindowViewModel;

        void Register<TWindowView, TWindowViewModel>(ScopeType scope)
            where TWindowView : Window, IWindowView
            where TWindowViewModel : IWindowViewModel;

        void Register(Type viewType, Type viewModelType, ScopeType scope);
    }
}