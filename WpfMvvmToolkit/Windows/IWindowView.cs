using System;
using System.ComponentModel;
using System.Windows;

namespace WpfMvvmToolkit.Windows
{
    public interface IWindowView
    {
        event RoutedEventHandler Loaded;
        event RoutedEventHandler Unloaded;
        event CancelEventHandler Closing;
        event EventHandler Closed;
        Window Owner { get; set; }
        WindowResult? Result { get; set; }
        object DataContext { get; set; }
        void Show();
        bool? ShowDialog();
        void Close();
    }
}
