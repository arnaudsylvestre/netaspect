using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.OnFinally.Method
{
    public class OnFinallyConstructorConstructorInfoParameterWithBadTypeTest :
        NetAspectTest<OnFinallyConstructorConstructorInfoParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the method parameter in the method OnFinallyConstructor of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.Reflection.ConstructorInfo",
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

            public void OnFinallyConstructor(int constructor)
            {
            }
        }
    }
}