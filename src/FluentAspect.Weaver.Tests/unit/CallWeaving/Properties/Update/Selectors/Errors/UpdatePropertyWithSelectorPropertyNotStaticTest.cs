using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Properties.Update.Selectors.Errors
{
    public class UpdatePropertyWithSelectorPropertyNotStaticTest :
        NetAspectTest<UpdatePropertyWithSelectorPropertyNotStaticTest.ClassToWeave>
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
            public string Property {get;set;}

            public void Weaved()
            {
                Property = "Dummy";
            }
        }

        public class MyAspect : Attribute
        {
            public static ClassToWeave Caller;
            public bool NetAspectAttribute = true;

            public void BeforeUpdateProperty(ClassToWeave caller)
            {
                Caller = caller;
            }

            public bool SelectProperty(string PropertyName)
            {
                return PropertyName == "Property";
            }
        }
    }
}