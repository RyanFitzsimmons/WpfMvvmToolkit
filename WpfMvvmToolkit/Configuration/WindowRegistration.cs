using System;

namespace WpfMvvmToolkit.Configuration
{
    internal class WindowRegistration
    {
        public WindowRegistration(Type viewType, Type viewModelType, ScopeType scope)
        {
            ViewType = viewType;
            ViewModelType = viewModelType;
            Scope = scope;
        }

        public Type ViewType { get; private set; }
        public Type ViewModelType { get; private set; }
        public ScopeType Scope { get; private set; }
    }
}
