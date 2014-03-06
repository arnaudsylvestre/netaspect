using System;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Instance;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Properties.Getter.Parameters.After.CalledParameterName
{
    public class AfterCallGetPropertyCalledParameterNameParameterWithRealTypeOutTest :
        NetAspectTest<AfterCallGetPropertyCalledParameterNameParameterWithRealTypeOutTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the instance parameter in the method After of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.Object or {1}",
                        typeof (AfterMethodInstanceParameterWithBadTypeTest.MyAspect).FullName,
                        typeof (AfterMethodInstanceParameterWithBadTypeTest.ClassToWeave).FullName));
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string Property { get; set; }

            public string Weaved(int param1)
            {
                return Property;
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void AfterGetProperty(out int calledParam1)
            {
                calledParam1 = 12;
            }
        }
    }
}