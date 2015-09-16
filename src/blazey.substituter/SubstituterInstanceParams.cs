using System;
using Castle.Windsor;
using Moq;

namespace blazey.substituter
{
    public class SubstituteInstanceParams<TService> where TService : class
    {
        internal static SubstituteInstanceParams<TService> Config(IWindsorContainer container,
            Action<SubstituteInstanceParams<TService>> config)
        {
            var args = new SubstituteInstanceParams<TService>();
            config(args);
            args.Register(container);
            return args;
        }

        private Func<TService> _factory;
        private Type _componentType;

        public void Instance(Func<TService> factory)
        {
            _factory = factory;
        }

        public void Instance(TService instance)
        {
            _factory = () => instance;
        }

        public void Stub()
        {
            _factory = Mock.Of<TService>;
        }

        public void Component<TComponent>() where TComponent : TService
        {
            _componentType = typeof (TComponent);
        }

        internal void Register(IWindsorContainer container)
        {
            RegisterComponent<TService>.Register(container, _factory, _componentType);
        }
    }
}