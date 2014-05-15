using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.ParameterName
{
    public class AfterMethodParameterNameParameterWithBadTypeTest :
        NetAspectTest<AfterMethodParameterNameParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Add(new ErrorReport.Error()
                {
                    Level = ErrorLevel.Error,
                    Message =
                    string.Format(
                        "the i parameter in the method After of the type '{0}' is declared with the type 'System.String' but it is expected to be {1}",
                        typeof(MyAspect).FullName, typeof(int), typeof(ClassToWeave))
                });
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