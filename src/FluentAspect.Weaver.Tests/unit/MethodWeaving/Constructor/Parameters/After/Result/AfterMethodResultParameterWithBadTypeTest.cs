using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.Result
{
    public class AfterConstructorResultParameterWithBadTypeTest :
        NetAspectTest<AfterConstructorResultParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the result parameter in the method After of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.String because the return type of the method Weaved in the type {1}",
                        typeof (MyAspect).FullName, typeof (ClassToWeave).FullName));
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string Weaved()
            {
                return "Hello";
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void AfterConstructor(int result)
            {
            }
        }
    }
}