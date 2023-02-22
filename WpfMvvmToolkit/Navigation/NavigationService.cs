using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WpfMvvmToolkit.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly Dictionary<INavigationHost, List<INavigationAware>> _viewModels = new();
        private readonly Dictionary<INavigationAware, INavigationHost> _currentViewModel = new();

        public async Task StartNavigation(INavigationHost host, INavigationAware viewModel, NavigationParameters? parameters = null)
        {
            if (_viewModels.ContainsKey(host))
            {
                throw new Exception("The host is already registered.");
            }

            if (parameters == null)
            {
                parameters = new();
            }

            _viewModels.Add(host, new());
            await NavigateToView(host, viewModel, parameters);
        }

        public async Task Navigate(INavigationAware from, INavigationAware to, NavigationParameters? parameters = null)
        {
            if (!_currentViewModel.ContainsKey(from))
            {
                throw new Exception($"The view is not the current view");
            }

            if (parameters == null)
            {
                parameters = new();
            }

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

            if (parameters == null)
            {
                parameters = new();
            }

            if (!from.CanNavigate(parameters))
            {
                return;
            }

            var host = await NavigateFromView(from, parameters);

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

        public async Task EndNavigation(INavigationAware from, NavigationParameters? parameters = null)
        {
            if (!_currentViewModel.ContainsKey(from))
            {
                throw new Exception($"The view is not the current view");
            }

            if (parameters == null)
            {
                parameters = new();
            }

            if (!from.CanNavigate(parameters))
            {
                return;
            }

            var host = await NavigateFromView(from, parameters);
            End(host, parameters);
        }

        public async Task EndNavigation(INavigationHost host)
        {
            var viewModel = GetCurrentViewModel(host);

            if (viewModel == null)
            {
                throw new KeyNotFoundException($"There are no active view models for this host.");
            }

            await EndNavigation(viewModel);
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
            await viewModel.OnNavigateTo(parameters).ConfigureAwait(false);
            _viewModels[host].Add(viewModel);
            _currentViewModel.Add(viewModel, host);
            host.DisplayedViewModel = viewModel;
        }

    }
}
