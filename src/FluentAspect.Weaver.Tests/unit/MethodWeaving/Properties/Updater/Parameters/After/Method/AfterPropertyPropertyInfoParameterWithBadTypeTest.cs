using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.After.Method
{
    public class AfterPropertyPropertyInfoParameterWithBadTypeTest :
        NetAspectTest<AfterPropertyPropertyInfoParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the Property parameter in the method AfterPropertySetMethodof the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.Reflection.PropertyInfo",
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

            public void AfterPropertySetMethod(int Property)
            {
            }
        }
    }
}