using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Fields.Update.Parameters.After.CallerParameters
{
    public class AfterCallUpdateFieldCallerParametersParameterWithObjectTypeTest :
        NetAspectTest<AfterCallUpdateFieldCallerParametersParameterWithObjectTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the callerParameters parameter in the method AfterUpdateField of the type '{0}' is declared with the type 'System.Object' but it is expected to be System.Object[]",
                        typeof (MyAspect).FullName,
                        typeof (ClassToWeave).FullName));
        }

        public class ClassToWeave
        {
            [MyAspect] public string Field;

            public void Weaved()
            {
                Field = "Dummy";
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void AfterUpdateField(object callerParameters)
            {
            }
        }
    }
}