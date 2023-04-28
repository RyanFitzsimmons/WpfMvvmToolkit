namespace WpfMvvmToolkit.Navigation
{
    public interface INavigationHost
    {
        INavigationAware? DisplayedViewModel { get; set; }
        void OnNavigationEnded(NavigationParameters parameters);
    }
}
