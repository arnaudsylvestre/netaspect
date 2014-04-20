using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.Instance
{
    public class AfterConstructorInstanceParameterWithRealTypeInStaticTest :
        NetAspectTest<AfterConstructorInstanceParameterWithRealTypeInStaticTest.ClassToWeave>
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
            static ClassToWeave()
            {
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void AfterConstructor(ClassToWeave instance)
            {
            }
        }
    }
}