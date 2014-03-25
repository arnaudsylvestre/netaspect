using System;
using FluentAspect.Weaver.Core.Errors;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.After.CallerParameters
{
    public class AfterCallGetFieldCallerParametersParameterWithBadTypeTest :
        NetAspectTest<AfterCallGetFieldCallerParametersParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the callerParameters parameter in the method AfterGetField of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.Object[]",
                        typeof (MyAspect).FullName));
        }

        public class ClassToWeave
        {
            [MyAspect] public string Field;

            public string Weaved()
            {
                return Field;
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void AfterGetField(int callerParameters)
            {
            }
        }
    }
}