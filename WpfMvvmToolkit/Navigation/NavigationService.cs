using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WpfMvvmToolkit.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly Dictionary<INavigationHost, List<INavigationAware>> _views = new();
        private readonly Dictionary<INavigationAware, INavigationHost> _currentView = new();

        public async Task Start(INavigationHost host, INavigationAware view, NavigationParameters parameters)
        {
            if (_views.ContainsKey(host))
            {
                throw new Exception("The host is already registered.");
            }

            _views.Add(host, new());
            await NavigateToView(host, view, parameters);
        }

        private async void View_GoBack(object? sender, NavigationParametersEventArgs e)
        {
            if (sender is not INavigationAware from)
            {
                throw new Exception($"The sender is not {nameof(INavigationAware)}.");
            }

            if (!_currentView.ContainsKey(from))
            {
                throw new Exception($"The view is not the current view");
            }

            if (!from.CanNavigate(e.Parameters))
            {
                return;
            }

            var host = await NavigateFromView(from, e.Parameters);

            var viewCount = _views[host].Count;
            if (viewCount < 2)
            {
                End(host, e.Parameters);
                return;
            }

            _views[host].RemoveAt(viewCount - 1);
            var to = _views[host].Last();
            await NavigateToView(host, to, e.Parameters);
        }

        private void End(INavigationHost host, NavigationParameters parameters)
        {
            _views.Remove(host);
            host.Close(parameters);
        }

        private async void View_NavigateTo(object? sender, NavigateEventArgs e)
        {
            if (sender is not INavigationAware from)
            {
                throw new Exception($"The sender is not {nameof(INavigationAware)}.");
            }

            if (!_currentView.ContainsKey(from))
            {
                throw new Exception($"The view is not the current view");
            }

            if (!from.CanNavigate(e.Parameters))
            {
                return;
            }

            var host = await NavigateFromView(from, e.Parameters);
            await NavigateToView(host, e.To, e.Parameters);
        }

        private async Task<INavigationHost> NavigateFromView(INavigationAware view, NavigationParameters parameters)
        {
            view.NavigateTo -= View_NavigateTo;
            view.GoBack -= View_GoBack;
            await view.OnNavigateFrom(parameters).ConfigureAwait(false);
            var host = _currentView[view];
            _currentView.Remove(view);
            return host;
        }

        private async Task NavigateToView(INavigationHost host, INavigationAware view, NavigationParameters parameters)
        {
            view.NavigateTo += View_NavigateTo;
            view.GoBack += View_GoBack;
            await view.OnNavigateTo(parameters).ConfigureAwait(false);
            _currentView.Add(view, host);
            host.DisplayedViewModel = view;
        }
    }
}
