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

        public WindowResult(IWindowViewModel viewModel, WindowParameters parameters, string button)
        {
            ViewModel = viewModel;
            Parameters = parameters;
            Button = button;
        }

        public IWindowViewModel ViewModel { get; }
        public WindowParameters Parameters { get; }
        public string Button { get; }
    }
}
