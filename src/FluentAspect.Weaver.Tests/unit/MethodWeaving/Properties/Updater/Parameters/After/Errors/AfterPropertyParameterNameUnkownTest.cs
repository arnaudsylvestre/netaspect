using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.After.Errors
{
    public class AfterPropertyParameterNameUnkownTest : NetAspectTest<AfterPropertyParameterNameUnkownTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return errorHandler => errorHandler.Add(new ErrorReport.Error()
                {
                    Level = ErrorLevel.Error,
                    Message = string.Format("The parameter 'unknown' is unknown")
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

            public void AfterPropertySetMethod(string unknown)
            {
            }
        }
    }
}