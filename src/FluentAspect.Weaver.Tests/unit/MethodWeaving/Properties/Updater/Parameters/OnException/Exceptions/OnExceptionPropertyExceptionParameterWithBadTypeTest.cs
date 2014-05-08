using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.OnException.Exceptions
{
    public class OnExceptionPropertyExceptionParameterWithBadTypeTest :
        NetAspectTest<OnExceptionPropertyExceptionParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the exception parameter in the method OnExceptionPropertySetMethod of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.Exception",
                        typeof (MyAspect).FullName));
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string Weaved
           {
              set { }
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void OnExceptionPropertySetMethod(int exception)
            {
            }
        }
    }
}