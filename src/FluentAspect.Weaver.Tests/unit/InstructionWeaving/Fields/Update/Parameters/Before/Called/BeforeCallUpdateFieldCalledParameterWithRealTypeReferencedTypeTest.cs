using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Update.Parameters.Before.Called
{
    public class BeforeCallUpdateFieldCalledParameterWithRealTypeReferencedTypeTest :
        NetAspectTest<BeforeCallUpdateFieldCalledParameterWithRealTypeReferencedTypeTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "impossible to ref/out the parameter 'called' in the method BeforeUpdateField of the type '{0}'",
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

            public void BeforeUpdateField(ref ClassToWeave called)
            {
            }
        }
    }
}