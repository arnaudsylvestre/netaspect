using System;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Parameters.After.Property
{
    public class AfterPropertyPropertyInfoParameterWithRealTypeReferencedTest :
        NetAspectTest<AfterPropertyPropertyInfoParameterWithRealTypeReferencedTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format("impossible to ref/out the parameter 'property' in the method AfterPropertyGetMethod of the type '{0}'",
                                  typeof (MyAspect).FullName));
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string Weaved
            {
                get { return "12"; }
            }
        }

        public class MyAspect : Attribute
        {
           public static PropertyInfo Property;
            public bool NetAspectAttribute = true;

            public void AfterPropertyGetMethod(ref PropertyInfo property)
            {
                Property = property;
            }
        }
    }
}