using System;
using Castle.MicroKernel;
using Castle.Windsor;

namespace blazey.substituter
{
    internal class TestSubstituteHandlerSelector : IHandlerSelector
    {
        private readonly IWindsorContainer _container;

        public TestSubstituteHandlerSelector(IWindsorContainer container)
        {
            _container = container;
        }

        public bool HasOpinionAbout(string key, Type service)
        {
            var testKey = TestSubstituteFacility.TestKey(service);
            return _container.Kernel.HasComponent(testKey);
        }

        public IHandler SelectHandler(string key, Type service, IHandler[] handlers)
        {
            var testKey = TestSubstituteFacility.TestKey(service);

            return _container.Kernel.GetHandler(testKey);
        }
    }
}