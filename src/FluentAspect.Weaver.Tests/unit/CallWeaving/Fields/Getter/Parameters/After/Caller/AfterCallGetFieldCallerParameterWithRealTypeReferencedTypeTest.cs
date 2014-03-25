using System;
using FluentAspect.Weaver.Core.Errors;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.After.Caller
{
    public class AfterCallGetFieldCallerParameterWithRealTypeReferencedTypeTest :
        NetAspectTest<AfterCallGetFieldCallerParameterWithRealTypeReferencedTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "impossible to ref/out the parameter 'caller' in the method AfterGetField of the type '{0}'",
                        typeof (MyAspect).FullName,
                        typeof (ClassToWeave).FullName));
        }

        public class ClassToWeave
        {
            [MyAspect] public string Field;

            public string Weaved()
            {
                return Field;
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void AfterGetField(ref ClassToWeave caller)
            {
            }
        }
    }
}