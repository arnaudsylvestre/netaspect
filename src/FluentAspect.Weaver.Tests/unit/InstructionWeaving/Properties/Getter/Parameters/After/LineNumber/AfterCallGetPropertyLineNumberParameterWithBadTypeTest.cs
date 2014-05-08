using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Getter.Parameters.After.LineNumber
{
    public class AfterCallGetPropertyLineNumberParameterWithBadTypeTest :
        NetAspectTest<AfterCallGetPropertyLineNumberParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the lineNumber parameter in the method AfterGetProperty of the type '{0}' is declared with the type 'System.String' but it is expected to be System.Int32",
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
            public bool NetAspectAttribute = true;

            public void AfterGetProperty(string lineNumber)
            {
            }
        }
    }
}