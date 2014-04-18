using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.OnException.Exceptions
{
    public class OnExceptionConstructorExceptionParameterWithBadTypeTest :
        NetAspectTest<OnExceptionConstructorExceptionParameterWithBadTypeTest.ClassToWeave>
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
            public ClassToWeave()
            {
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void OnExceptionConstructor(int exception)
            {
            }
        }
    }
}