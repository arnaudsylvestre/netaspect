using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.Result
{
    public class AfterConstructorResultParameterWithRealTypeOutTest :
        NetAspectTest<AfterConstructorResultParameterWithRealTypeOutTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format("impossible to out the parameter 'result' in the method After of the type '{0}'",
                                  typeof (MyAspect).FullName));
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string Weaved()
            {
                return "NeverUsedValue";
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void AfterConstructor(out string result)
            {
                result = "MyNewValue";
            }
        }
    }
}