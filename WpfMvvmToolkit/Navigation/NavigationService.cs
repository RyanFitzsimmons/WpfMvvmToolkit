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

        public async Task Start(INavigationHost host, INavigationAware viewModel, NavigationParameters parameters)
        {
            if (_viewModels.ContainsKey(host))
            {
                throw new Exception("The host is already registered.");
            }

            _viewModels.Add(host, new());
            await NavigateToView(host, viewModel, parameters);
        }

        private async void ViewModel_GoBack(object? sender, NavigationParametersEventArgs e)
        {
            if (sender is not INavigationAware from)
            {
                throw new Exception($"The sender is not {nameof(INavigationAware)}.");
            }

            if (!_currentViewModel.ContainsKey(from))
            {
                throw new Exception($"The view is not the current view");
            }

            if (!from.CanNavigate(e.Parameters))
            {
                return;
            }

            var host = await NavigateFromView(from, e.Parameters);

            var viewCount = _viewModels[host].Count;
            if (viewCount < 2)
            {
                End(host, e.Parameters);
                return;
            }

            _viewModels[host].RemoveAt(viewCount - 1);
            var to = _viewModels[host].Last();

            _viewModels[host].RemoveAt(_viewModels[host].Count - 1);
            await NavigateToView(host, to, e.Parameters);
        }

        private void End(INavigationHost host, NavigationParameters parameters)
        {
            _viewModels.Remove(host);
            host.DisplayedViewModel = null;
            host.OnNavigationEnded(parameters);
        }

        private async void ViewModel_NavigateTo(object? sender, NavigateEventArgs e)
        {
            if (sender is not INavigationAware from)
            {
                throw new Exception($"The sender is not {nameof(INavigationAware)}.");
            }

            if (!_currentViewModel.ContainsKey(from))
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

        private async Task<INavigationHost> NavigateFromView(INavigationAware viewModel, NavigationParameters parameters)
        {
            viewModel.NavigateTo -= ViewModel_NavigateTo;
            viewModel.GoBack -= ViewModel_GoBack;
            viewModel.EndNavigation -= ViewModel_EndNavigation;
            await viewModel.OnNavigateFrom(parameters).ConfigureAwait(false);
            var host = _currentViewModel[viewModel];
            _currentViewModel.Remove(viewModel);
            return host;
        }

        private async Task NavigateToView(INavigationHost host, INavigationAware viewModel, NavigationParameters parameters)
        {
            viewModel.NavigateTo += ViewModel_NavigateTo;
            viewModel.GoBack += ViewModel_GoBack;
            viewModel.EndNavigation += ViewModel_EndNavigation;
            await viewModel.OnNavigateTo(parameters).ConfigureAwait(false);
            _viewModels[host].Add(viewModel);
            _currentViewModel.Add(viewModel, host);
            host.DisplayedViewModel = viewModel;
        }

        private async void ViewModel_EndNavigation(object? sender, NavigationParametersEventArgs e)
        {
            if (sender is not INavigationAware from)
            {
                throw new Exception($"The sender is not {nameof(INavigationAware)}.");
            }

            if (!_currentViewModel.ContainsKey(from))
            {
                throw new Exception($"The view is not the current view");
            }

            if (!from.CanNavigate(e.Parameters))
            {
                return;
            }

            var host = await NavigateFromView(from, e.Parameters);
            End(host, e.Parameters);
        }
    }
}
