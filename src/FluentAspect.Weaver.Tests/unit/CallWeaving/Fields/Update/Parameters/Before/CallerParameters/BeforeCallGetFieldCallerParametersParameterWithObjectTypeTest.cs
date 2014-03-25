using System;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.Before.Instance;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Updater.Parameters.Before.CallerParameters
{
    public class BeforeCallUpdateFieldCallerParametersParameterWithObjectTypeTest :
        NetAspectTest<BeforeCallUpdateFieldCallerParametersParameterWithObjectTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the callerParameters parameter in the method BeforeUpdateField of the type '{0}' is declared with the type 'System.Object' but it is expected to be System.Object[]",
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

            public void BeforeUpdateField(object callerParameters)
            {
            }
        }
    }
}