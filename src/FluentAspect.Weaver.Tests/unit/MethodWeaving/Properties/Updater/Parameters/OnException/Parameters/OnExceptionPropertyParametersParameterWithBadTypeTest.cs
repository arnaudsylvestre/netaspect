using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.OnException.Parameters
{
    public class OnExceptionPropertyParametersParameterWithBadTypeTest :
        NetAspectTest<OnExceptionPropertyParametersParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the parameters parameter in the Property OnException of the type '{0}' is declared with the type 'System.Int32' but it is expected to be {1}",
                        typeof (MyAspect).FullName, typeof (object[])));
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

            public void OnExceptionPropertySetMethod(int parameters)
            {
            }
        }
    }
}