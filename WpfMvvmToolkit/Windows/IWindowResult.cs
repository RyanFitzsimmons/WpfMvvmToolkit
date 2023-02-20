namespace WpfMvvmToolkit.Windows
{
    public interface IWindowResult
    {
        NavigationParameters Parameters { get; }
        string Button { get; }
        IWindowViewModel ViewModel { get; }
    }
}