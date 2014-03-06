using System;
using FluentAspect.Weaver.Core.Errors;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.OnException.Exceptions
{
    public class OnExceptionMethodExceptionParameterWithBadTypeTest :
        NetAspectTest<OnExceptionMethodExceptionParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the exception parameter in the method OnException of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.Exception",
                        typeof (MyAspect).FullName));
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

            public void OnException(int exception)
            {
            }
        }
    }
}