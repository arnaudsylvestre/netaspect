using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.ParameterName
{
    public class AfterMethodParameterNameParameterWithBadTypeTest :
        NetAspectTest<AfterMethodParameterNameParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the i parameter in the method After of the type '{0}' is declared with the type 'System.String' but it is expected to be {1} because of the type of this parameter in the method Weaved of the type {2}",
                        typeof (MyAspect).FullName, typeof (int), typeof (ClassToWeave)));
        }

        public class ClassToWeave
        {
            [MyAspect]
            public void Weaved(int i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void After(string i)
            {
            }
        }
    }
}