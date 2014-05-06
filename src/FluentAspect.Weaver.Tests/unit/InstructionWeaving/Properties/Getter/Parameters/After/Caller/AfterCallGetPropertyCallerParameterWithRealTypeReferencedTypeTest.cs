using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Getter.Parameters.After.Caller
{
    public class AfterCallGetPropertyCallerParameterWithRealTypeReferencedTypeTest :
        NetAspectTest<AfterCallGetPropertyCallerParameterWithRealTypeReferencedTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "impossible to ref/out the parameter 'caller' in the method AfterGetProperty of the type '{0}'",
                        typeof (MyAspect).FullName,
                        typeof (ClassToWeave).FullName));
        }

        public class ClassToWeave
        {
            [MyAspect] public string Property {get; set; }

            public string Weaved()
            {
                return Property;
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void AfterGetProperty(ref ClassToWeave caller)
            {
            }
        }
    }
}