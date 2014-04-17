using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.After.Instance
{
    public class AfterPropertyInstanceParameterWithRealTypeInStaticTest :
        NetAspectTest<AfterPropertyInstanceParameterWithRealTypeInStaticTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format("the instance parameter can not be used for static method interceptors"));
        }

        public class ClassToWeave
        {
            [MyAspect]
            public static string Weaved
           {
              set { }
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void AfterPropertySetMethod(ClassToWeave instance)
            {
            }
        }
    }
}