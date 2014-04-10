using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Methods.Parameters.After.LineNumber
{
    public class AfterCallMethodLineNumberParameterWithBadTypeTest :
        NetAspectTest<AfterCallMethodLineNumberParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the lineNumber parameter in the method AfterCallMethod of the type '{0}' is declared with the type 'System.String' but it is expected to be System.Int32",
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

            public void AfterCallMethod(string lineNumber)
            {
            }
        }
    }
}