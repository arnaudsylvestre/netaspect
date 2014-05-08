using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Update.Parameters.Before.FilePath
{
    public class BeforeCallUpdateFieldFilePathParameterWithBadTypeTest :
        NetAspectTest<BeforeCallUpdateFieldFilePathParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the filePath parameter in the method BeforeUpdateField of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.String",
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
            public static int FilePath;
            public bool NetAspectAttribute = true;

            public void BeforeUpdateField(int filePath)
            {
                FilePath = filePath;
            }
        }
    }
}