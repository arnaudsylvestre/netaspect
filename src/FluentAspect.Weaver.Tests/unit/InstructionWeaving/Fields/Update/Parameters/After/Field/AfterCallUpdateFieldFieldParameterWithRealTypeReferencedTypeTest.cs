using System;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Update.Parameters.After.Field
{
    public class AfterCallUpdateFieldFieldParameterWithRealTypeReferencedTypeTest :
        NetAspectTest<AfterCallUpdateFieldFieldParameterWithRealTypeReferencedTypeTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "impossible to ref/out the parameter 'field' in the method AfterUpdateField of the type '{0}'",
                        typeof (MyAspect).FullName));
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

            public void AfterUpdateField(ref FieldInfo field)
            {
            }
        }
    }
}