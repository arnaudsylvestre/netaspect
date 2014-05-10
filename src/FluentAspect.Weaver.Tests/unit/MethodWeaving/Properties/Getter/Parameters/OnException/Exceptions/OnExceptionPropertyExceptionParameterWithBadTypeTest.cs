using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Parameters.OnException.Exceptions
{
    public class OnExceptionPropertyExceptionParameterWithBadTypeTest :
        NetAspectTest<OnExceptionPropertyExceptionParameterWithBadTypeTest.ClassToWeave>
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
                        "the exception parameter in the method OnExceptionPropertyGetMethod of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.Exception",
                        typeof(MyAspect).FullName)
                });
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string Weaved
            {
                get { return "12"; }
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void OnExceptionPropertyGetMethod(int exception)
            {
            }
        }
    }
}