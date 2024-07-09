using Ninject;
using WpfMvvmToolkit.Configuration;

namespace WpfMvvmToolkit.Ninject
{
    public class ServiceContainer : IServiceContainer
    {
        private IKernel _kernel;

        public ServiceContainer()
        {
            _kernel = new StandardKernel();
        }

        public object Get(Type type)
        {
            return _kernel.Get(type);
        }

        public T Get<T>()
        {
            return _kernel.Get<T>();
        }

        public void Register(Type t1, Type t2, ScopeType scope)
        {
            switch (scope)
            {
                case ScopeType.Singleton:
                    _kernel.Bind(t1).To(t2).InSingletonScope();
                    break;
                case ScopeType.Transient:
                    _kernel.Bind(t1).To(t2).InTransientScope();
                    break;
                default:
                    throw new ArgumentException($"Unknown scope {scope}");
            }
        }

        public void Register<T1, T2>(ScopeType scope) where T2 : T1
        {
            Register(typeof(T1), typeof(T2), scope);
        }

        public void RegisterToSelf<T1>(ScopeType scope)
        {
            RegisterToSelf(typeof(T1), scope);
        }

        public void RegisterToSelf(Type type, ScopeType scope)
        {
            switch (scope)
            {
                case ScopeType.Singleton:
                    _kernel.Bind(type).ToSelf().InSingletonScope();
                    break;
                case ScopeType.Transient:
                    _kernel.Bind(type).ToSelf().InTransientScope();
                    break;
                default:
                    throw new ArgumentException($"Unknown scope {scope}");
            }
        }

        public void RegisterConstant<T1>(T1 constant) where T1 : class
        {
            _kernel.Bind<T1>().ToConstant(constant);
        }

        public void Register<T1>(Func<T1> factoryDelegate) where T1 : class
        {
            _kernel.Bind<T1>().ToMethod<T1>((context) => factoryDelegate());
        }
    }
}