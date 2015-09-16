using blazey.substituter.specs.ContextSpecification;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Shouldly;

namespace blazey.substituter.specs
{
    public class when_component_is_substituted_with_a_stub : context_specification
    {
        private Establish that = () =>
        {
            _container = new WindsorContainer();
            _container.Register(Component.For<IService>().ImplementedBy<TheComponent>());

            _container.AddFacility<SubstituterFacility>(config =>
            {
                config
                    .WithContainer(_container)
                    .Substitute<IService>(sub => sub.Stub());
            });
        };

        private Because when_resolved = () => _component = _container.Resolve<IService>();
        private Then should_substitute_resolved_component = () => _component.ShouldNotBeOfType<TheComponent>();
        private static object _component;
        private static IWindsorContainer _container;
    }

    public class TheSubstitute : IService
    {
    }

    public class TheComponent : IService
    {
    }

    public interface IService
    {
    }

    public class when_component_is_substituted_with_an_instance : context_specification
    {
        private Establish that = () =>
        {
            _container = new WindsorContainer();
            _container.Register(Component.For<IService>().ImplementedBy<TheComponent>());

            _container.AddFacility<SubstituterFacility>(config =>
            {
                config
                    .WithContainer(_container)
                    .Substitute<IService>(sub => sub.Instance(new TheSubstitute()));
            });
        };

        private Because when_resolved = () => _component = _container.Resolve<IService>();
        private Then should_substitute_resolved_component = () => _component.ShouldBeOfType<TheSubstitute>();
        private static object _component;
        private static IWindsorContainer _container;
    }

    public class when_component_is_substituted_with_a_factory_instance : context_specification
    {
        private Establish that = () =>
        {
            _container = new WindsorContainer();
            _container.Register(Component.For<IService>().ImplementedBy<TheComponent>());

            _container.AddFacility<SubstituterFacility>(config =>
            {
                config
                    .WithContainer(_container)
                    .Substitute<IService>(sub => sub.Instance(()=> new TheSubstitute()));
            });
        };

        private Because when_resolved = () => _component = _container.Resolve<IService>();
        private Then should_substitute_resolved_component = () => _component.ShouldBeOfType<TheSubstitute>();
        private static object _component;
        private static IWindsorContainer _container;
    }

    public class when_component_is_substituted_by_component_type : context_specification
    {
        private Establish that = () =>
        {
            _container = new WindsorContainer();
            _container.Register(Component.For<IService>().ImplementedBy<TheComponent>());

            _container.AddFacility<SubstituterFacility>(config =>
            {
                config
                    .WithContainer(_container)
                    .Substitute<IService>(sub => sub.Component<TheSubstitute>());
            });
        };

        private Because when_resolved = () => _component = _container.Resolve<IService>();
        private Then should_substitute_resolved_component = () => _component.ShouldBeOfType<TheSubstitute>();
        private static object _component;
        private static IWindsorContainer _container;
    }
}