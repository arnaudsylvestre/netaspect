using System;
using FluentAspect.Weaver.Core.Errors;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Methods.Parameters.After.FieldValue
{
    public class AfterCallMethodFieldValueParameterWithBadTypeTest :
        NetAspectTest<AfterCallMethodFieldValueParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the instance parameter in the method After of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.Object or {1}",
                        typeof (string).FullName, typeof (string).FullName));
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
            public static int FieldValue;
            public bool NetAspectAttribute = true;

            public void AfterCallMethod(int fieldValue)
            {
                FieldValue = fieldValue;
            }
        }
    }
}