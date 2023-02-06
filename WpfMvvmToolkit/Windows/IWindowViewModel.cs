using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace WpfMvvmToolkit.Windows
{
    public interface IWindowViewModel
    {
        string Title { get; }
        event Action<IWindowResult>? Close;
        void OnOpen(WindowParameters? parameters);
        void OnClosing(CancelEventArgs e);
        void OnClose();
        Task Load();
        void Unload();

    }
}
