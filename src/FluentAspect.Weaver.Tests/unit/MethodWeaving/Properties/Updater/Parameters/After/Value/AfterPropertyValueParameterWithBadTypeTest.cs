using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.After.Value
{
    public class AfterPropertyValueParameterWithBadTypeTest :
        NetAspectTest<AfterPropertyValueParameterWithBadTypeTest.ClassToWeave>
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
                        "the value parameter in the method AfterPropertySetMethod of the type '{0}' is declared with the type 'System.String' but it is expected to be {1} because of the type of this parameter in the method set_Weaved of the type {2}",
                        typeof(MyAspect).FullName, typeof(int), typeof(ClassToWeave))
                });
        }

        public class ClassToWeave
        {
            [MyAspect]
            public int Weaved
           {
              set { }
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void AfterPropertySetMethod(string value)
            {
            }
        }
    }
}