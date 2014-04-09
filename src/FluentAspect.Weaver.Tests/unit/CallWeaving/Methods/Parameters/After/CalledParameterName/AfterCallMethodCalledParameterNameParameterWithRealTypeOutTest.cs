using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Methods.Parameters.After.CalledParameterName
{
    public class AfterCallMethodCalledParameterNameParameterWithRealTypeOutTest :
        NetAspectTest<AfterCallMethodCalledParameterNameParameterWithRealTypeOutTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "impossible to ref/out the parameter 'calledParam1' in the method AfterCallMethod of the type '{0}'",
                        typeof(MyAspect).FullName));
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string Method(int param1)
            {
                return "Hello";
            }

            public string Weaved()
            {
                return Method(12);
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void AfterCallMethod(out int calledParam1)
            {
                calledParam1 = 12;
            }
        }
    }
}