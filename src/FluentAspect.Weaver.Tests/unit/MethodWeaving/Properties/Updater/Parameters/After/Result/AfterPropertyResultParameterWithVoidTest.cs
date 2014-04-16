using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.After.Result
{
    public class AfterPropertyResultParameterWithVoidTest :
        NetAspectTest<AfterPropertyResultParameterWithVoidTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "Impossible to use the result parameter in the method AfterPropertySetMethodof the type '{0}' because the return type of the Property Weaved in the type {1} is void",
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

            public void AfterPropertySetMethod(int result)
            {
            }
        }
    }
}