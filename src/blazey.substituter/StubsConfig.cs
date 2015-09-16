using System;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Moq;

namespace blazey.substituter
{
    internal class StubsConfig
    {
        readonly ISet<Type> _service = new HashSet<Type>();

        internal StubsConfig Stub<TService>()
        {
            _service.Add(typeof (TService));
            return this;
        }

        internal void Register(IWindsorContainer container)
        {
            foreach (var type in _service)
            {
                container.Register(Component.For(type).Instance(typeof (Mock<>).MakeGenericType(type)));
            }
        }

    }
}