using System;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Instance;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Properties.Update.Parameters.After.FileName
{
    public class AfterCallUpdatePropertyFileNameParameterWithBadTypeTest :
        NetAspectTest<AfterCallUpdatePropertyFileNameParameterWithBadTypeTest.ClassToWeave>
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
            public static int FileName;
            public bool NetAspectAttribute = true;

            public void AfterUpdateProperty(int fileName)
            {
                FileName = fileName;
            }
        }
    }
}