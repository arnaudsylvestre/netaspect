using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Update.Parameters.After.CallerParameterName
{
    public class AfterCallUpdateFieldCallerParameterNameParameterWithRealTypeOutTest :
        NetAspectTest<AfterCallUpdateFieldCallerParameterNameParameterWithRealTypeOutTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "impossible to out the parameter 'callerParam1' in the method AfterUpdateField of the type '{0}'",
                        typeof (MyAspect).FullName));
        }

        public class ClassToWeave
        {
            [MyAspect] public string Field;

            public void Weaved(int param1)
            {
                Field = "Dummy";
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void AfterUpdateField(out int callerParam1)
            {
                callerParam1 = 12;
            }
        }
    }
}