using System;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.OnException.Constructor
{
    public class OnExceptionConstructorConstructorInfoParameterWithRealTypeReferencedTest :
        NetAspectTest<OnExceptionConstructorConstructorInfoParameterWithRealTypeReferencedTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "impossible to ref/out the parameter 'constructor' in the method OnExceptionConstructor of the type '{0}'",
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
            public static MethodBase Method;
            public bool NetAspectAttribute = true;

            public void OnExceptionConstructor(ref MethodBase constructor)
            {
                Method = constructor;
            }
        }
    }
}