using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Properties.Getter.Parameters.Before.CallerParameters
{
    public class BeforeCallGetPropertyCallerParametersParameterWithObjectTypeTest :
        NetAspectTest<BeforeCallGetPropertyCallerParametersParameterWithObjectTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the callerParameters parameter in the method BeforeGetProperty of the type '{0}' is declared with the type 'System.Object' but it is expected to be System.Object[]",
                        typeof (MyAspect).FullName,
                        typeof (ClassToWeave).FullName));
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

            public void BeforeGetProperty(object callerParameters)
            {
            }
        }
    }
}