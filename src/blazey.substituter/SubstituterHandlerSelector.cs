using System;
using Castle.MicroKernel;
using Castle.Windsor;

namespace blazey.substituter
{
    internal class SubstituterHandlerSelector : IHandlerSelector
    {
        private readonly IWindsorContainer _container;

        public SubstituterHandlerSelector(IWindsorContainer container)
        {
            _container = container;
        }

        public bool HasOpinionAbout(string key, Type service)
        {
            var testKey = SubstituterFacility.TestKey(service);
            return _container.Kernel.HasComponent(testKey);
        }

        public IHandler SelectHandler(string key, Type service, IHandler[] handlers)
        {
            var testKey = SubstituterFacility.TestKey(service);

            return _container.Kernel.GetHandler(testKey);
        }
    }
}