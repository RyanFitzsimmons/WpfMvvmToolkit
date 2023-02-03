using System;

namespace WpfMvvmToolkit.Configuration
{
    public interface IServiceContainer
    {
        object Get(Type type);
        T Get<T>();
        void Register<T1, T2>(ScopeType scope) where T2 : T1;
        void Register(Type t1, Type t2, ScopeType scope);
        void RegisterToSelf<T1>(ScopeType scope);
        void RegisterToSelf(Type type, ScopeType scope);
        void RegisterConstant<T1>(T1 constant);
    }
}
