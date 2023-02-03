using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace WpfMvvmToolkit.Windows
{
    public interface IWindowViewModel
    {
        string Title { get; }
        event Action<IWindowViewModel>? Close;
        void OnOpen(WindowParameters? parameters);
        void OnClosing(CancelEventArgs e);
        void OnClose(Action<WindowParameters>? callback);
        Task Load();
        void Unload();

    }
}
