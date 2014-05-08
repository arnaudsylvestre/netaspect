using System;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.After.Property
{
    public class AfterPropertyPropertyInfoParameterWithRealTypeReferencedTest :
        NetAspectTest<AfterPropertyPropertyInfoParameterWithRealTypeReferencedTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format("impossible to ref/out the parameter 'Property' in the method AfterPropertySetMethod of the type '{0}'",
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
           public static PropertyInfo Property;
            public bool NetAspectAttribute = true;

            public void AfterPropertySetMethod(ref PropertyInfo Property)
            {
                Property = Property;
            }
        }
    }
}