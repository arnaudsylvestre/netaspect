using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Update.Selectors.Errors
{
    public class UpdatePropertyWithSelectorPropertyNotBooleanTest :
        NetAspectTest<UpdatePropertyWithSelectorPropertyNotBooleanTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
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

            public void BeforeSetProperty(ClassToWeave caller)
            {
                Caller = caller;
            }

            public static int SelectProperty(string PropertyName)
            {
                return 1;
            }
        }
    }
}