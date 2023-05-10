using System.Threading.Tasks;

namespace WpfMvvmToolkit.Navigation
{
    public interface INavigationService
    {
        bool GetHostExists(INavigationHost host);
        Task StartNavigation(INavigationHost host, INavigationAware viewModel, NavigationParameters? parameters = null, bool keepHistory = true);
        Task Navigate(INavigationAware from, INavigationAware to, NavigationParameters? parameters = null);
        Task NavigateBack(INavigationAware from, NavigationParameters? parameters = null);
        Task EndNavigation(INavigationAware from, NavigationParameters? parameters = null, bool force = false);
        Task EndNavigation(INavigationHost host);
        bool CanClose(INavigationHost host);
        INavigationHost? GetHost(INavigationAware viewModel);
    }
}