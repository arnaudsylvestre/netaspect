using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Update.Parameters.Before.Caller
{
    public class BeforeCallUpdateFieldCallerParameterWithBadTypeTest :
        NetAspectTest<BeforeCallUpdateFieldCallerParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Add(new ErrorReport.Error()
                {
                    Level = ErrorLevel.Error,
                    Message =
                    string.Format(
                        "the caller parameter in the method BeforeUpdateField of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.Object or {1}",
                        typeof(MyAspect).FullName,
                        typeof(ClassToWeave).FullName)
                });
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
            public static int Caller;
            public bool NetAspectAttribute = true;

            public void BeforeUpdateField(int caller)
            {
                Caller = caller;
            }
        }
    }
}