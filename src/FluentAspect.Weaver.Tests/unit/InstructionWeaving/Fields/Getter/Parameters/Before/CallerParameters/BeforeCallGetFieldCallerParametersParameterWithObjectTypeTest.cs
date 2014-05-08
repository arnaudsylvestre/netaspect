using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Getter.Parameters.Before.CallerParameters
{
    public class BeforeCallGetFieldCallerParametersParameterWithObjectTypeTest :
        NetAspectTest<BeforeCallGetFieldCallerParametersParameterWithObjectTypeTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the callerParameters parameter in the method BeforeGetField of the type '{0}' is declared with the type 'System.Object' but it is expected to be System.Object[]",
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

            public void BeforeGetField(object callerParameters)
            {
            }
        }
    }
}