using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Methods.Parameters.After.CallerParameters
{
    public class AfterCallMethodCallerParametersParameterWithBadTypeTest :
        NetAspectTest<AfterCallMethodCallerParametersParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the callerParameters parameter in the method AfterCallMethod of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.Object[]",
                        typeof(MyAspect).FullName, typeof(ClassToWeave).FullName));
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string Method()
            {
                return "Hello";
            }

            public string Weaved()
            {
                return Method();
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void AfterCallMethod(int callerParameters)
            {
            }
        }
    }
}