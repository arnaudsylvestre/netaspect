using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.OnException.Parameters
{
    public class OnExceptionMethodParametersParameterWithBadTypeTest :
        NetAspectTest<OnExceptionMethodParametersParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the parameters parameter in the method OnException of the type '{0}' is declared with the type 'System.Int32' but it is expected to be {1}",
                        typeof (MyAspect).FullName, typeof (object[])));
        }

        public class ClassToWeave
        {
            [MyAspect]
            public void Weaved()
            {
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void OnException(int parameters)
            {
            }
        }
    }
}