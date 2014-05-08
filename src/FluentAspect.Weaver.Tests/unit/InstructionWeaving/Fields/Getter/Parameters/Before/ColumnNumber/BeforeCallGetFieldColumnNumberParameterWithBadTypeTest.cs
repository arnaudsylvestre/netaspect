using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Getter.Parameters.Before.ColumnNumber
{
    public class BeforeCallGetFieldColumnNumberParameterWithBadTypeTest :
        NetAspectTest<BeforeCallGetFieldColumnNumberParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the columnNumber parameter in the method BeforeGetField of the type '{0}' is declared with the type 'System.String' but it is expected to be System.Int32",
                        typeof (MyAspect).FullName));
        }

        public class ClassToWeave
        {
            [MyAspect] public string Field;

            public string Weaved()
            {
                return Field;
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void BeforeGetField(string columnNumber)
            {
            }
        }
    }
}