using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Getter.Parameters.Before.Caller
{
    public class BeforeCallGetFieldCallerParameterWithRealTypeReferencedTypeTest :
        NetAspectTest<BeforeCallGetFieldCallerParameterWithRealTypeReferencedTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "impossible to ref/out the parameter 'caller' in the method BeforeGetField of the type '{0}'",
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

            public void BeforeGetField(ref ClassToWeave caller)
            {
            }
        }
    }
}