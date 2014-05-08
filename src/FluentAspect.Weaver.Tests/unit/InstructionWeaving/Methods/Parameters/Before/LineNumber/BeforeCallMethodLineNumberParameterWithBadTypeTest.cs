using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Methods.Parameters.Before.LineNumber
{
    public class BeforeCallMethodLineNumberParameterWithBadTypeTest :
        NetAspectTest<BeforeCallMethodLineNumberParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the lineNumber parameter in the method BeforeCallMethod of the type '{0}' is declared with the type 'System.String' but it is expected to be System.Int32",
                        typeof(MyAspect).FullName));
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string Method()
            {
                return "Hello";
            }

            public string Weaved()
            {
                return Method();
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void BeforeCallMethod(string lineNumber)
            {
            }
        }
    }
}