using System;

namespace WpfMvvmToolkit.Navigation
{
    public class NavigateEventArgs : EventArgs
    {
        public NavigateEventArgs(INavigationAware to, NavigationParameters? parameters = null)
        {
            To = to;
            Parameters = parameters ?? new();
        }

        public INavigationAware To { get; set; }
        public NavigationParameters Parameters { get; set; }
    }
}
