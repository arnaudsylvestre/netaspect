using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Instance
{
    public class AfterMethodInstanceParameterWithRealTypeInStaticTest :
        NetAspectTest<AfterMethodInstanceParameterWithRealTypeInStaticTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format("the instance parameter can not be used for static method interceptors"));
        }

        public class ClassToWeave
        {
            [MyAspect]
            public static void Weaved()
            {
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void After(ClassToWeave instance)
            {
            }
        }
    }
}