using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Methods.Parameters.Before.ColumnNumber
{
    public class BeforeCallMethodColumnNumberParameterWithRealTypeReferencedTypeTest :
        NetAspectTest<BeforeCallMethodColumnNumberParameterWithRealTypeReferencedTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "impossible to ref/out the parameter 'columnNumber' in the method BeforeCallMethod of the type '{0}'",
                        typeof(MyAspect).FullName, typeof(string).FullName));
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

            public void BeforeCallMethod(ref int columnNumber)
            {
            }
        }
    }
}