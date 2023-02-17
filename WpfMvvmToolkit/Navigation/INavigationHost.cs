namespace WpfMvvmToolkit.Navigation
{
    public interface INavigationHost
    {
        INavigationAware? DisplayedViewModel { get; set; }
        void Close(NavigationParameters parameters);
    }
}
