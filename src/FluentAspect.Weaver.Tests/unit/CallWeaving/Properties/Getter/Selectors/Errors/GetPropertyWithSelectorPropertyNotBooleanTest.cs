using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Properties.Getter.Selectors.Errors
{
    public class GetPropertyWithSelectorPropertyNotBooleanTest :
        NetAspectTest<GetPropertyWithSelectorPropertyNotBooleanTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "The selector SelectProperty in the aspect {0} must return boolean value",
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

            public static int SelectProperty(string propertyName)
            {
                return 1;
            }
        }
    }
}