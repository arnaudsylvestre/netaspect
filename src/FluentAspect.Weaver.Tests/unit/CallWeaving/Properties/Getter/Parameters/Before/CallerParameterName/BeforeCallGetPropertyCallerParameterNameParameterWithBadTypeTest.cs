using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Properties.Getter.Parameters.Before.CallerParameterName
{
    public class BeforeCallGetPropertyCallerParameterNameParameterWithBadTypeTest :
        NetAspectTest<BeforeCallGetPropertyCallerParameterNameParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the callerParam1 parameter in the method BeforeGetProperty of the type '{0}' is declared with the type 'System.String' but it is expected to be System.Int32 because of the type of this parameter in the method Weaved of the type {1}",
                        typeof (MyAspect).FullName,
                        typeof (ClassToWeave).FullName));
        }

        public class ClassToWeave
        {
            [MyAspect] public string Property {get; set; }

            public string Weaved(int param1)
            {
                return Property;
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void BeforeGetProperty(string callerParam1)
            {
            }
        }
    }
}