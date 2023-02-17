using System;

namespace WpfMvvmToolkit.Navigation
{
    public class NavigationParametersEventArgs : EventArgs
    {
        public NavigationParametersEventArgs(NavigationParameters? parameters = null)
        {
            Parameters = parameters ?? new();
        }

        public NavigationParameters Parameters { get; set; }
    }
}
