using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace WpfMvvmToolkit.Windows
{
    public interface IWindowViewModel : ILoadable
    {
        string Title { get; }
        event Action<WindowResult>? Close;
        void OnOpen(NavigationParameters parameters);
        void OnClosing(CancelEventArgs e);
        Task OnClose();
    }
}
