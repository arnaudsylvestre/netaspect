using System;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.OnFinally.Method
{
    public class OnFinallyConstructorConstructorInfoParameterWithRealTypeReferencedTest :
        NetAspectTest<OnFinallyConstructorConstructorInfoParameterWithRealTypeReferencedTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "impossible to ref/out the parameter 'constructor' in the method OnFinallyConstructor of the type '{0}'",
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
            public static ConstructorInfo Method;
            public bool NetAspectAttribute = true;

            public void OnFinallyConstructor(ref ConstructorInfo constructor)
            {
                Method = constructor;
            }
        }
    }
}