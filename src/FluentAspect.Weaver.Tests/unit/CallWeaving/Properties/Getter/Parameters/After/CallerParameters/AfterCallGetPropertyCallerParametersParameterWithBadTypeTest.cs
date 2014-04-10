using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Properties.Getter.Parameters.After.CallerParameters
{
    public class AfterCallGetPropertyCallerParametersParameterWithBadTypeTest :
        NetAspectTest<AfterCallGetPropertyCallerParametersParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the callerParameters parameter in the method AfterGetProperty of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.Object[]",
                        typeof (MyAspect).FullName));
        }

        public class ClassToWeave
        {
            [MyAspect] public string Property {get; set; }

            public string Weaved()
            {
                return Property;
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void AfterGetProperty(int callerParameters)
            {
            }
        }
    }
}