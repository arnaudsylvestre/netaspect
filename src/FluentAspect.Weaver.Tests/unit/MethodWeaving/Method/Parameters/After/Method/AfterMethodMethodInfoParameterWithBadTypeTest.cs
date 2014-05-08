using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Method
{
    public class AfterMethodMethodInfoParameterWithBadTypeTest :
        NetAspectTest<AfterMethodMethodInfoParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the method parameter in the method After of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.Reflection.MethodBase",
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
            public bool NetAspectAttribute = true;

            public void After(int method)
            {
            }
        }
    }
}