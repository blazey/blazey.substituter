using System;
using Castle.MicroKernel.Facilities;
using Castle.Windsor;

namespace blazey.substituter
{
    public class SubstituterFacility : AbstractFacility
    {
        private IWindsorContainer _container;

        public SubstituterFacility WithContainer(IWindsorContainer container)
        {
            _container = container;
            return this;
        }

        public SubstituterFacility Substitute<TService>(Action<SubstituteInstanceParams<TService>> config)
            where TService : class
        {
            SubstituteInstanceParams<TService>.Config(_container, config);
            return this;
        }

        internal static string TestKey(Type service)
        {
            return string.Format("{0}::{1}", typeof (SubstituterFacility).Name, service.FullName);
        }

        protected override void Init()
        {
            Kernel.AddHandlerSelector(new SubstituterHandlerSelector(_container));
        }
    }
}