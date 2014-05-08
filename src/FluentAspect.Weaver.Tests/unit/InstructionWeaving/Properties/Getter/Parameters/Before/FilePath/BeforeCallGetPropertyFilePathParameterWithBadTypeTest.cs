using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Getter.Parameters.Before.FilePath
{
    public class BeforeCallGetPropertyFilePathParameterWithBadTypeTest :
        NetAspectTest<BeforeCallGetPropertyFilePathParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the filePath parameter in the method BeforeGetProperty of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.String",
                        typeof (MyAspect).FullName));
        }

        public class ClassToWeave
        {
            [MyAspect] public string Property {get; set; }

            public string Weaved()
            {
                return Property;
            }
        }

        public class MyAspect : Attribute
        {
            public static int FilePath;
            public bool NetAspectAttribute = true;

            public void BeforeGetProperty(int filePath)
            {
                FilePath = filePath;
            }
        }
    }
}