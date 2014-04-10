using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Properties.Getter.Parameters.Before.ColumnNumber
{
    public class BeforeCallGetPropertyColumnNumberParameterWithBadTypeTest :
        NetAspectTest<BeforeCallGetPropertyColumnNumberParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the columnNumber parameter in the method BeforeGetProperty of the type '{0}' is declared with the type 'System.String' but it is expected to be System.Int32",
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

            public void BeforeGetProperty(string columnNumber)
            {
            }
        }
    }
}