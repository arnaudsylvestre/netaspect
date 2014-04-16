using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.After.Result
{
    public class AfterPropertyResultParameterWithReferencedObjectTest :
        NetAspectTest<AfterPropertyResultParameterWithReferencedObjectTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the result parameter in the Property After of the type '{0}' is declared with the type 'System.Object&' but it is expected to be System.String because the return type of the Property Weaved in the type {1}",
                        typeof (MyAspect).FullName, typeof (ClassToWeave).FullName));
        }


        public class ClassToWeave
        {
            [MyAspect]
            public string Weaved()
            {
                return "NeverUsedValue";
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;


            public void After(ref object result)
            {
            }
        }
    }
}