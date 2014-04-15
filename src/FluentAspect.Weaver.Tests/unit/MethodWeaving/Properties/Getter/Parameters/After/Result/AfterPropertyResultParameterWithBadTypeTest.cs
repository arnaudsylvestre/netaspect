using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Parameters.After.Result
{
    public class AfterPropertyResultParameterWithBadTypeTest :
        NetAspectTest<AfterPropertyResultParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the result parameter in the property After of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.String because the return type of the property Weaved in the type {1}",
                        typeof (MyAspect).FullName, typeof (ClassToWeave).FullName));
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

            public void After(int result)
            {
            }
        }
    }
}