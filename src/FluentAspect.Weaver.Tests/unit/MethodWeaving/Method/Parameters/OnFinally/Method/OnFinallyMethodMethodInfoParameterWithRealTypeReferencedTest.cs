using System;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.OnFinally.Method
{
    public class OnFinallyMethodMethodInfoParameterWithRealTypeReferencedTest :
        NetAspectTest<OnFinallyMethodMethodInfoParameterWithRealTypeReferencedTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "impossible to ref/out the parameter 'method' in the method OnFinally of the type '{0}'",
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
            public static MethodInfo Method;
            public bool NetAspectAttribute = true;

            public void OnFinally(ref MethodInfo method)
            {
                Method = method;
            }
        }
    }
}