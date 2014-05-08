using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.After.Instance
{
    public class AfterPropertyInstanceParameterWithBadTypeTest :
        NetAspectTest<AfterPropertyInstanceParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the instance parameter in the method AfterPropertySetMethod of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.Object or {1}",
                        typeof (MyAspect).FullName, typeof (ClassToWeave).FullName));
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

            public void AfterPropertySetMethod(int instance)
            {
            }
        }
    }
}