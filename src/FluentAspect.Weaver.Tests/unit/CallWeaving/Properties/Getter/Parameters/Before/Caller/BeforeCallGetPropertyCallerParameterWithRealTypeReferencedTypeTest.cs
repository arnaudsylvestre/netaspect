using System;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.Before.Instance;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Properties.Getter.Parameters.Before.Caller
{
    public class BeforeCallGetPropertyCallerParameterWithRealTypeReferencedTypeTest :
        NetAspectTest<BeforeCallGetPropertyCallerParameterWithRealTypeReferencedTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the instance parameter in the method Before of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.Object or {1}",
                        typeof (BeforeMethodInstanceParameterWithBadTypeTest.MyAspect).FullName,
                        typeof (BeforeMethodInstanceParameterWithBadTypeTest.ClassToWeave).FullName));
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string Property { get; set; }

            public string Weaved()
            {
                return Property;
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void BeforeGetProperty(ref ClassToWeave caller)
            {
            }
        }
    }
}