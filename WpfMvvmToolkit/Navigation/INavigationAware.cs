using System;
using System.Threading.Tasks;

namespace WpfMvvmToolkit.Navigation
{
    public interface INavigationAware
    {
        event EventHandler<NavigateEventArgs>? NavigateTo;
        event EventHandler<NavigationParametersEventArgs>? GoBack;

        bool CanNavigate(NavigationParameters parameters);
        Task OnNavigateTo(NavigationParameters parameters);
        Task OnNavigateFrom(NavigationParameters parameters);
    }
}
