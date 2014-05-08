using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Update.Parameters.Before.Called
{
    public class BeforeCallUpdatePropertyCalledParameterWithRealTypeReferencedTypeTest :
        NetAspectTest<BeforeCallUpdatePropertyCalledParameterWithRealTypeReferencedTypeTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "impossible to ref/out the parameter 'called' in the method BeforeSetProperty of the type '{0}'",
                        typeof (MyAspect).FullName));
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

            public void BeforeSetProperty(ref ClassToWeave called)
            {
            }
        }
    }
}