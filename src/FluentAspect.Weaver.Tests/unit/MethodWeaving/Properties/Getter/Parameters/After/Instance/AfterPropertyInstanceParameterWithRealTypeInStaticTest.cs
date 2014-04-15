using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Parameters.After.Instance
{
    public class AfterPropertyInstanceParameterWithRealTypeInStaticTest :
        NetAspectTest<AfterPropertyInstanceParameterWithRealTypeInStaticTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format("the instance parameter can not be used for static property interceptors"));
        }

        public class ClassToWeave
        {
            [MyAspect]
            public static string Weaved
            {
                get { return "12"; }
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void AfterPropertyGetMethod(ClassToWeave instance)
            {
            }
        }
    }
}