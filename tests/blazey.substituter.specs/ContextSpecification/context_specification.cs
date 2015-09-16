using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace blazey.substituter.specs.ContextSpecification
{
    [DebuggerStepThrough]
    [TestFixture]
    public abstract class context_specification
    {
        public delegate void Because();

        public delegate void Cleanup();

        public delegate void Establish();

        public delegate void Then();

        protected Exception Exception;

        private const BindingFlags _bindingFlags = BindingFlags.Instance
                                                   | BindingFlags.NonPublic
                                                   | BindingFlags.FlattenHierarchy;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            InvokeEstablish();
            InvokeBecause();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            InvokeCleanup();
        }

        private void InvokeEstablish()
        {
            var types = new Stack<Type>();
            var type = GetType();

            do
            {
                types.Push(type);
                type = type.BaseType;
            } while (type != typeof (context_specification));

            foreach (var t in types)
            {
                var establishFieldInfo = t.GetFields(_bindingFlags)
                    .SingleOrDefault(x => x.FieldType.Name.Equals(typeof (Establish).Name));

                Delegate establish = null;

                if (establishFieldInfo != null) establish = establishFieldInfo.GetValue(this) as Delegate;
                if (establish != null) Exception = Catch.Exception(() => establish.DynamicInvoke(null));
            }
        }

        private void InvokeBecause()
        {
            var t = GetType();

            var becauseFieldInfo =
                t.GetFields(_bindingFlags)
                    .SingleOrDefault(x => x.FieldType.Name.Equals(typeof (Because).Name));

            Delegate because = null;

            if (becauseFieldInfo != null) because = becauseFieldInfo.GetValue(this) as Delegate;
            if (because != null) Exception = Catch.Exception(() => because.DynamicInvoke(null));
        }

        private void InvokeCleanup()
        {
            try
            {
                var t = GetType();

                var cleanupFieldInfo =
                    t.GetFields(_bindingFlags)
                        .SingleOrDefault(x => x.FieldType.Name.Equals(typeof (Cleanup).Name));

                Delegate cleanup = null;

                if (cleanupFieldInfo != null) cleanup = cleanupFieldInfo.GetValue(this) as Delegate;
                if (cleanup != null) cleanup.DynamicInvoke(null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public IEnumerable GetObservations()
        {
            var t = GetType();

            var category = (CategoryAttribute) t.GetCustomAttributes(typeof (CategoryAttribute), true).FirstOrDefault();
            string categoryName = null;

            if (category != null)
                categoryName = category.Name;

            var itFieldInfos = t.GetFields(_bindingFlags)
                .Where(x => x.FieldType.Name.Equals(typeof (Then).Name));

            return itFieldInfos
                .Select(fieldInfo => new TestCaseData(fieldInfo.GetValue(this))
                    .SetName(fieldInfo.Name)
                    .SetCategory(categoryName));
        }

        [Test, TestCaseSource("GetObservations")]
        public void Observation(Then observation)
        {
            if (Exception != null)
            {
                throw Exception;
            }

            observation();
        }
    }
}