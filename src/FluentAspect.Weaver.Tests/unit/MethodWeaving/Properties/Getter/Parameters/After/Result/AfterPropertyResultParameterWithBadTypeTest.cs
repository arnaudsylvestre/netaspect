using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Parameters.After.Result
{
    public class AfterPropertyResultParameterWithBadTypeTest :
        NetAspectTest<AfterPropertyResultParameterWithBadTypeTest.ClassToWeave>
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
                        "the result parameter in the method AfterPropertyGetMethod of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.String because the return type of the method get_Weaved in the type {1}",
                        typeof(MyAspect).FullName, typeof(ClassToWeave).FullName)
                });
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string Weaved
            {

                get { return "Hello"; }
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void AfterPropertyGetMethod(int result)
            {
            }
        }
    }
}