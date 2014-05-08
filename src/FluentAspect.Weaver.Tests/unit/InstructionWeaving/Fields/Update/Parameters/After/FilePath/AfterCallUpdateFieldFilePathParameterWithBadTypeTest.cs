using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Update.Parameters.After.FilePath
{
    public class AfterCallUpdateFieldFilePathParameterWithBadTypeTest :
        NetAspectTest<AfterCallUpdateFieldFilePathParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the filePath parameter in the method AfterUpdateField of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.String",
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

            public void AfterUpdateField(int filePath)
            {
                FilePath = filePath;
            }
        }
    }
}