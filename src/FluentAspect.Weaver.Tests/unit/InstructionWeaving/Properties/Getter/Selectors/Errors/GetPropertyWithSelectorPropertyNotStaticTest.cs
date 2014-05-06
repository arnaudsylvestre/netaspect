using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Getter.Selectors.Errors
{
    public class GetPropertyWithSelectorPropertyNotStaticTest :
        NetAspectTest<GetPropertyWithSelectorPropertyNotStaticTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "The selector SelectProperty in the aspect {0} must be static",
                        typeof(MyAspect).FullName));
        }

        public class ClassToWeave
        {
            public string Property {get; set; }

            public string Weaved()
            {
                return Property;
            }
        }

        public class MyAspect : Attribute
        {
            public static ClassToWeave Caller;
            public bool NetAspectAttribute = true;

            public void BeforeGetProperty(ClassToWeave caller)
            {
                Caller = caller;
            }

            public bool SelectProperty(string propertyName)
            {
                return propertyName == "Property";
            }
        }
    }
}