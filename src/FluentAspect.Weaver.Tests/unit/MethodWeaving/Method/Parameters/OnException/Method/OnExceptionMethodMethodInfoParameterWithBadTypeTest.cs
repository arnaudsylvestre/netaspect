using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.OnException.Method
{
    public class OnExceptionMethodMethodInfoParameterWithBadTypeTest :
        NetAspectTest<OnExceptionMethodMethodInfoParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the method parameter in the method OnException of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.Reflection.MethodInfo",
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

            public void OnException(int method)
            {
            }
        }
    }
}