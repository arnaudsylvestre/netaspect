using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.Parameters
{
    public class AfterConstructorParametersParameterWithRealTypeReferencedTest :
        NetAspectTest<AfterConstructorParametersParameterWithRealTypeReferencedTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "impossible to ref/out the parameter 'parameters' in the method After of the type '{0}'",
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
            public static object[] Parameters;
            public bool NetAspectAttribute = true;

            public void AfterConstructor(ref object[] parameters)
            {
                Parameters = parameters;
            }
        }
    }
}