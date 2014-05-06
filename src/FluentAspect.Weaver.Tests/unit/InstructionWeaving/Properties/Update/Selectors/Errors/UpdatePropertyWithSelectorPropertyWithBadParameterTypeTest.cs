using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Update.Selectors.Errors
{
    public class UpdatePropertyWithSelectorPropertyWithBadParameterTypeTest :
        NetAspectTest<UpdatePropertyWithSelectorPropertyWithBadParameterTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "The parameter PropertyName in the method SelectProperty of the aspect {0} is expected to be System.String",
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

            public static bool SelectProperty(int PropertyName)
            {
                return true;
            }
        }
    }
}