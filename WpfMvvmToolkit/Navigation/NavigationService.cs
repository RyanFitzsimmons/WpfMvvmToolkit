using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WpfMvvmToolkit.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly Dictionary<INavigationHost, bool> _keepHistoryLookup = new();
        private readonly Dictionary<INavigationHost, List<INavigationAware>> _viewModels = new();
        private readonly Dictionary<INavigationAware, INavigationHost> _currentViewModel = new();

        public bool GetHostExists(INavigationHost host)
        {
            return _viewModels.ContainsKey(host);
        }

        public async Task StartNavigation(INavigationHost host, INavigationAware viewModel, NavigationParameters? parameters = null, bool keepHistory = true)
        {
            if (_viewModels.ContainsKey(host))
            {
                throw new Exception("The host is already registered.");
            }

            parameters ??= new();
            _keepHistoryLookup.Add(host, keepHistory);
            _viewModels.Add(host, new());
            await NavigateToView(host, viewModel, parameters);
        }

        public async Task Navigate(INavigationAware from, INavigationAware to, NavigationParameters? parameters = null)
        {
            if (!_currentViewModel.ContainsKey(from))
            {
                throw new Exception($"The view is not the current view");
            }

            parameters ??= new();

            if (!from.CanNavigate(parameters))
            {
                return;
            }

            var host = await NavigateFromView(from, parameters);
            await NavigateToView(host, to, parameters);
        }

        public async Task NavigateBack(INavigationAware from, NavigationParameters? parameters = null)
        {
            if (!_currentViewModel.ContainsKey(from))
            {
                throw new Exception($"The view is not the current view");
            }

            parameters ??= new();

            if (!from.CanNavigate(parameters))
            {
                return;
            }

            var host = await NavigateFromView(from, parameters);

            if (!_keepHistoryLookup[host])
            {
                throw new Exception("NavigateBack cannot be used when no history is kept");
            }

            var viewCount = _viewModels[host].Count;
            if (viewCount < 2)
            {
                End(host, parameters);
                return;
            }

            _viewModels[host].RemoveAt(viewCount - 1);
            var to = _viewModels[host].Last();

            _viewModels[host].RemoveAt(_viewModels[host].Count - 1);
            await NavigateToView(host, to, parameters);
        }

        public async Task EndNavigation(INavigationAware from, NavigationParameters? parameters = null, bool force = false)
        {
            if (!_currentViewModel.ContainsKey(from))
            {
                throw new Exception($"The view is not the current view");
            }

            parameters ??= new();

            if (!force && !from.CanNavigate(parameters))
            {
                return;
            }

            var host = await NavigateFromView(from, parameters);
            End(host, parameters);
        }

        public async Task EndNavigation(INavigationHost host)
        {
            var viewModel = GetCurrentViewModel(host) ??
                throw new KeyNotFoundException($"There are no active view models for this host.");

            await EndNavigation(viewModel, force: true);
        }

        public bool CanClose(INavigationHost host)
        {
            /// This can happen if the navigation has ended before the window is closed.
            if (!_viewModels.ContainsKey(host))
            {
                return true;
            }

            var currentViewModel = GetCurrentViewModel(host);

            if (currentViewModel == null)
            {
                return true;
            }

            return currentViewModel.CanNavigate(new());
        }

        public INavigationHost? GetHost(INavigationAware viewModel)
        {
            foreach (var navigation in _viewModels)
            {
                if (navigation.Value.Contains(viewModel))
                {
                    return navigation.Key;
                }
            }

            return null;
        }

        private INavigationAware? GetCurrentViewModel(INavigationHost host)
        {
            foreach (var viewModel in _viewModels[host])
            {
                if (!_currentViewModel.ContainsKey(viewModel))
                {
                    continue;
                }

                return viewModel;
            }

            return null;
        }

        private void End(INavigationHost host, NavigationParameters parameters)
        {
            _keepHistoryLookup.Remove(host);
            _viewModels.Remove(host);
            host.DisplayedViewModel = null;
            host.OnNavigationEnded(parameters);
        }

        private async Task<INavigationHost> NavigateFromView(INavigationAware viewModel, NavigationParameters parameters)
        {
            await viewModel.OnNavigateFrom(parameters).ConfigureAwait(false);
            var host = _currentViewModel[viewModel];
            _currentViewModel.Remove(viewModel);
            return host;
        }

        private async Task NavigateToView(INavigationHost host, INavigationAware viewModel, NavigationParameters parameters)
        {
            if (_keepHistoryLookup[host])
            {
                _viewModels[host].Add(viewModel);
            }

            _currentViewModel.Add(viewModel, host);
            host.DisplayedViewModel = viewModel;

            await viewModel.OnNavigateTo(parameters).ConfigureAwait(false);
        }

    }
}
