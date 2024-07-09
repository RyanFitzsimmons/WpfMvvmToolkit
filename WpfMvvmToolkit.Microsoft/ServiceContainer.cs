using Microsoft.Extensions.DependencyInjection;
using WpfMvvmToolkit.Configuration;

namespace WpfMvvmToolkit.Microsoft;

public class ServiceContainer : IServiceContainer
{
    private readonly IServiceCollection _services;
    private IServiceProvider? _provider;

    public ServiceContainer(IServiceCollection services)
    {
        _services = services;
    }

    internal void SetServiceProvider(IServiceProvider services)
    {
        _provider = services;
    }

    public object? Get(Type type)
    {
        if (_provider == null)
        {
            throw new Exception("The service provider hasn't been built");
        }

        return _provider.GetService(type);
    }

    public T? Get<T>()
    {
        if (_provider == null)
        {
            throw new Exception("The service provider hasn't been built");
        }

        return _provider.GetService<T>();
    }

    public void Register<T1, T2>(ScopeType scope) where T2 : T1
    {
        switch (scope)
        {
            case ScopeType.Transient:
                _services.AddTransient(typeof(T1), typeof(T2));
                break;
            case ScopeType.Singleton:
                _services.AddSingleton(typeof(T1), typeof(T2));
                break;
        }
    }

    public void Register(Type t1, Type t2, ScopeType scope)
    {
        switch (scope)
        {
            case ScopeType.Transient:
                _services.AddTransient(t1, t2);
                break;
            case ScopeType.Singleton:
                _services.AddSingleton(t1, t2);
                break;
            default:
                throw new NotImplementedException();
        }
    }

    public void RegisterToSelf<T1>(ScopeType scope)
    {
        Register(typeof(T1), typeof(T1), scope);
    }

    public void RegisterToSelf(Type type, ScopeType scope)
    {
        Register(type, type, scope);
    }

    public void RegisterConstant<T1>(T1 constant) where T1 : class
    {
        _services.AddSingleton(typeof(T1), constant!);
    }

    public void Register<T1>(Func<T1> factoryDelegate) where T1 : class
    {
        _services.AddTransient(typeof(T1), provider => factoryDelegate());
    }
}