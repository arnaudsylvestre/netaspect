using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Properties.Update.Parameters.Before.FilePath
{
    public class BeforeCallUpdatePropertyFilePathParameterWithRealTypeReferencedTypeTest :
        NetAspectTest<BeforeCallUpdatePropertyFilePathParameterWithRealTypeReferencedTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "impossible to ref/out the parameter 'filePath' in the method BeforeUpdateProperty of the type '{0}'",
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

            public void BeforeUpdateProperty(ref string filePath)
            {
            }
        }
    }
}