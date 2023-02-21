using System.Threading.Tasks;

namespace WpfMvvmToolkit.Navigation
{
    public interface INavigationService
    {
        Task StartNavigation(INavigationHost host, INavigationAware viewModel, NavigationParameters? parameters = null);
        Task Navigate(INavigationAware from, INavigationAware to, NavigationParameters? parameters = null);
        Task NavigateBack(INavigationAware from, NavigationParameters? parameters = null);
        Task EndNavigation(INavigationAware from, NavigationParameters? parameters = null);
    }
}