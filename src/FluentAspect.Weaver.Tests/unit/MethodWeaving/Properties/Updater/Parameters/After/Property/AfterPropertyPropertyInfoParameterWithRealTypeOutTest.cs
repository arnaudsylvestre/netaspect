using System;
using System.Collections.Generic;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.After.Property
{
    public class AfterPropertyPropertyInfoParameterWithRealTypeOutTest :
        NetAspectTest<AfterPropertyPropertyInfoParameterWithRealTypeOutTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Add(new ErrorReport.Error()
                {
                    Level = ErrorLevel.Error,
                    Message =
                    string.Format("impossible to ref/out the parameter 'Property' in the method AfterPropertySetMethod of the type '{0}'",
                                  typeof(MyAspect).FullName)
                });
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

            public void AfterPropertySetMethod(out PropertyInfo Property)
            {
                Property = null;
            }
        }
    }
}