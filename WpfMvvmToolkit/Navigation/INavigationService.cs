﻿using System.Threading.Tasks;

namespace WpfMvvmToolkit.Navigation
{
    public interface INavigationService
    {
        Task Start(INavigationHost host, INavigationAware view, NavigationParameters parameters);
    }
}