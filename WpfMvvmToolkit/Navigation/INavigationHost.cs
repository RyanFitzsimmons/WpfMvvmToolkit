using System.Threading.Tasks;

namespace WpfMvvmToolkit.Navigation
{
    public interface INavigationHost
    {
        INavigationAware? DisplayedViewModel { get; set; }
        Task OnNavigationEnded(NavigationParameters parameters);
    }
}
