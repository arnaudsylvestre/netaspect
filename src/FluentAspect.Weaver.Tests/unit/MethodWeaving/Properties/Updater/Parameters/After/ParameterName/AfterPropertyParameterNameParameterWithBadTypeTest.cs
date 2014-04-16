using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.After.ParameterName
{
    public class AfterPropertyParameterNameParameterWithBadTypeTest :
        NetAspectTest<AfterPropertyParameterNameParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the i parameter in the Property After of the type '{0}' is declared with the type 'System.String' but it is expected to be {1} because of the type of this parameter in the Property Weaved of the type {2}",
                        typeof (MyAspect).FullName, typeof (int), typeof (ClassToWeave)));
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

            public void AfterPropertySetMethod(string i)
            {
            }
        }
    }
}