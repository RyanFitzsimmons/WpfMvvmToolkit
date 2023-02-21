using System;
using System.Threading.Tasks;

namespace WpfMvvmToolkit.Navigation
{
    public interface INavigationAware
    {
        bool CanNavigate(NavigationParameters parameters);
        Task OnNavigateTo(NavigationParameters parameters);
        Task OnNavigateFrom(NavigationParameters parameters);
    }
}
