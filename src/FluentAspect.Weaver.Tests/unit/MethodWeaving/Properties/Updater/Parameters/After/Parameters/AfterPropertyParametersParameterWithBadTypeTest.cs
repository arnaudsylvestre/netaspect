using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.After.Parameters
{
    public class AfterPropertyParametersParameterWithBadTypeTest :
        NetAspectTest<AfterPropertyParametersParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the parameters parameter in the method AfterPropertySetMethodof the type '{0}' is declared with the type 'System.Int32' but it is expected to be {1}",
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

            public void AfterPropertySetMethod(int parameters)
            {
            }
        }
    }
}