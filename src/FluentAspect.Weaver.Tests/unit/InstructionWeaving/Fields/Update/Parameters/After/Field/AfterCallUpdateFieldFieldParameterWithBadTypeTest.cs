using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Update.Parameters.After.Field
{
    public class AfterCallUpdateFieldFieldParameterWithBadTypeTest :
        NetAspectTest<AfterCallUpdateFieldFieldParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the field parameter in the method AfterUpdateField of the type '{0}' is declared with the type 'System.String' but it is expected to be System.Reflection.FieldInfo",
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

            public void AfterUpdateField(string field)
            {
            }
        }
    }
}