using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Update.Parameters.Before.CallerParameters
{
    public class BeforeCallUpdatePropertyCallerParametersParameterWithObjectTypeTest :
        NetAspectTest<BeforeCallUpdatePropertyCallerParametersParameterWithObjectTypeTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the callerParameters parameter in the method BeforeSetProperty of the type '{0}' is declared with the type 'System.Object' but it is expected to be System.Object[]",
                        typeof (MyAspect).FullName,
                        typeof (ClassToWeave).FullName));
        }

        public class ClassToWeave
        {
            [MyAspect] public string Property {get;set;}

            public void Weaved()
            {
                Property = "Dummy";
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void BeforeSetProperty(object callerParameters)
            {
            }
        }
    }
}