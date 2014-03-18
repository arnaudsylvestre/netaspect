using System;
using FluentAspect.Weaver.Core.Errors;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Methods.Parameters.After.Caller
{
    public class AfterCallMethodCallerParameterWithRealTypeReferencedTypeTest :
        NetAspectTest<AfterCallMethodCallerParameterWithRealTypeReferencedTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "impossible to ref/out the parameter 'caller' in the method AfterCallMethod of the type '{0}'",
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

            public void AfterCallMethod(ref ClassToWeave caller)
            {
            }
        }
    }
}