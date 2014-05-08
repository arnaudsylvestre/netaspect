using System;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Methods.Parameters.After.CallerMethod
{
    public class AfterCallMethodCallerMethodParameterWithRealTypeReferencedTypeTest :
        NetAspectTest<AfterCallMethodCallerMethodParameterWithRealTypeReferencedTypeTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "impossible to ref/out the parameter 'callerMethod' in the method AfterCallMethod of the type '{0}'",
                        typeof (MyAspect).FullName));
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

            public void AfterCallMethod(ref MethodBase callerMethod)
            {
            }
        }
    }
}