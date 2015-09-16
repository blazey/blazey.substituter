using System;

namespace blazey.substituter.specs.ContextSpecification
{
    public class when_using_context_specification : context_specification
    {
        private Establish establish = () => { _value = typeof (when_using_context_specification); };

        private Because because = () => _exception = Catch.Exception(() => _name = _value.Name);

        private Then should_equal_type_name = () => _name.ShouldBe(typeof (when_using_context_specification).Name);

        private Then should_throw = () => _exception.ShouldBe(null);

        private static Type _value;
        private static string _name;
        private static Exception _exception;
    }
}