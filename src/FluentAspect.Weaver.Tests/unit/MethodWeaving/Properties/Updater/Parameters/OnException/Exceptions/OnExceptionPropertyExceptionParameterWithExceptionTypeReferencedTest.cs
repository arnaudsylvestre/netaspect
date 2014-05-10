using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.OnException.Exceptions
{
    public class OnExceptionPropertyExceptionParameterWithExceptionTypeReferencedTest :
        NetAspectTest<OnExceptionPropertyExceptionParameterWithExceptionTypeReferencedTest.ClassToWeave>
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
                        "impossible to ref/out the parameter 'exception' in the method OnExceptionPropertySetMethod of the type '{0}'",
                        typeof(MyAspect).FullName)
                });
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

            public void OnExceptionPropertySetMethod(ref Exception exception)
            {
            }
        }
    }
}