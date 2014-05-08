using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Update.Parameters.After.FileName
{
    public class AfterCallUpdatePropertyFileNameParameterWithRealTypeReferencedTypeTest :
        NetAspectTest<AfterCallUpdatePropertyFileNameParameterWithRealTypeReferencedTypeTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "impossible to ref/out the parameter 'fileName' in the method AfterSetProperty of the type '{0}'",
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

            public void AfterSetProperty(ref string fileName)
            {
            }
        }
    }
}