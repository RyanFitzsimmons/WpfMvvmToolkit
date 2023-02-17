using System;
using System.ComponentModel;

namespace WpfMvvmToolkit.Windows
{
    public interface IWindowViewModel : ILoadable
    {
        string Title { get; }
        event Action<IWindowResult>? Close;
        void OnOpen(WindowParameters parameters);
        void OnClosing(CancelEventArgs e);
        void OnClose();

    }
}
