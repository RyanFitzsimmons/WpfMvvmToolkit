using System;
using System.Threading.Tasks;

namespace WpfMvvmToolkit.Navigation
{
    public interface INavigationAware
    {
        event EventHandler<NavigateEventArgs>? NavigateTo;
        event EventHandler<NavigationParametersEventArgs>? GoBack;
        event EventHandler<NavigationParametersEventArgs>? EndNavigation;

        bool CanNavigate(NavigationParameters parameters);
        Task OnNavigateTo(NavigationParameters parameters);
        Task OnNavigateFrom(NavigationParameters parameters);
    }
}
