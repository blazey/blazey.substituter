using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace blazey.substituter
{
    public class RegisterComponent<TService> where TService : class
    {
        private readonly Func<TService> _factory;
        private readonly Type _componentType;

        public static void Register(IWindsorContainer container, Func<TService> factory, Type componentType)
        {
            if (null == componentType)
            {
                new RegisterComponent<TService>(factory).Register(container);
            }
            else
            {
                new RegisterComponent<TService>(componentType).Register(container);
            }
        }

        private RegisterComponent(Func<TService> factory)
        {
            _factory = factory;
        }

        private RegisterComponent(Type componentType)
        {
            _componentType = componentType;
        }


        internal void Register(IWindsorContainer container)
        {
            var service = typeof (TService);
            var key = SubstituterFacility.TestKey(service);

            var componentRegistration = Component.For<TService>().Named(key);

            if (null == _componentType)
            {
                componentRegistration.UsingFactoryMethod(_factory);
            }
            else
            {
                componentRegistration.ImplementedBy(_componentType);
            }

            container.Register(componentRegistration);
        }
    }
}