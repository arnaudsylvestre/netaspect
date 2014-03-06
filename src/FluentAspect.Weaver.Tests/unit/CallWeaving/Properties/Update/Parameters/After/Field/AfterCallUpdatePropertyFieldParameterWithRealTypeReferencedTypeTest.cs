using System;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Instance;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Properties.Update.Parameters.After.Field
{
    public class AfterCallUpdatePropertyFieldParameterWithRealTypeReferencedTypeTest :
        NetAspectTest<AfterCallUpdatePropertyFieldParameterWithRealTypeReferencedTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the instance parameter in the method After of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.Object or {1}",
                        typeof (AfterMethodInstanceParameterWithBadTypeTest.MyAspect).FullName,
                        typeof (AfterMethodInstanceParameterWithBadTypeTest.ClassToWeave).FullName));
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string Property { get; set; }

            public void Weaved()
            {
                Property = "Hello";
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void AfterUpdateProperty(ref FieldInfo field)
            {
            }
        }
    }
}