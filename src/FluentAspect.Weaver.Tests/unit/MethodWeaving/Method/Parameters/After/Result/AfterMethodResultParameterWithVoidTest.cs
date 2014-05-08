using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Result
{
    public class AfterMethodResultParameterWithVoidTest :
        NetAspectTest<AfterMethodResultParameterWithVoidTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "Impossible to use the result parameter in the method After of the type '{0}' because the return type of the method Weaved in the type {1} is void",
                        typeof (MyAspect).FullName, typeof (ClassToWeave).FullName));
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
            public bool NetAspectAttribute = true;

            public void After(int result)
            {
            }
        }
    }
}