namespace WpfMvvmToolkit.Windows
{
    public interface IWindowResult
    {
        WindowParameters Parameters { get; }
        string Button { get; }
        IWindowViewModel ViewModel { get; }
    }
}