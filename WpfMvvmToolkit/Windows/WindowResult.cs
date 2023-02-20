namespace WpfMvvmToolkit.Windows
{
    public class WindowResult : IWindowResult
    {
        public WindowResult(IWindowViewModel viewModel)
        {
            ViewModel = viewModel;
            Parameters = new();
            Button = "None";
        }

        public WindowResult(IWindowViewModel viewModel, NavigationParameters parameters, string button)
        {
            ViewModel = viewModel;
            Parameters = parameters;
            Button = button;
        }

        public IWindowViewModel ViewModel { get; }
        public NavigationParameters Parameters { get; }
        public string Button { get; }
    }
}
