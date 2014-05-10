using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.OnFinally.Parameters
{
    public class OnFinallyMethodParametersParameterWithBadTypeTest :
        NetAspectTest<OnFinallyMethodParametersParameterWithBadTypeTest.ClassToWeave>
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
                        "the parameters parameter in the method OnFinally of the type '{0}' is declared with the type 'System.Int32' but it is expected to be {1}",
                        typeof(MyAspect).FullName, typeof(object[]))
                });
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

            public void OnFinally(int parameters)
            {
            }
        }
    }
}