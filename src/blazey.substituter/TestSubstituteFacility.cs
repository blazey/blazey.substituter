using System;
using Castle.MicroKernel.Facilities;
using Castle.Windsor;

namespace blazey.substituter
{
    internal class TestSubstituteFacility : AbstractFacility
    {
        private IWindsorContainer _container;

        public TestSubstituteFacility WithContainer(IWindsorContainer container)
        {
            _container = container;
            return this;
        }

        public TestSubstituteFacility Stubs(StubsConfig stubs)
        {
            stubs.Register(_container);
            return this;
        }

        public TestSubstituteFacility Substitute<TService>(Action<TestSubstituteInstanceParams<TService>> config)
            where TService : class
        {
            TestSubstituteInstanceParams<TService>.Config(_container, config);
            return this;
        }

        internal static string TestKey(Type service)
        {
            return string.Format("{0}::{1}", typeof (TestSubstituteFacility).Name, service.FullName);
        }

        protected override void Init()
        {
            Kernel.AddHandlerSelector(new TestSubstituteHandlerSelector(_container));
        }
    }
}