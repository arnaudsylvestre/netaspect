using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Methods.Parameters.Before.CalledParameters
{
    public class BeforeCallMethodCalledParametersParameterWithBadTypeTest :
        NetAspectTest<BeforeCallMethodCalledParametersParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the calledParameters parameter in the method BeforeCallMethod of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.Object[]",
                        typeof(MyAspect).FullName, typeof(ClassToWeave).FullName));
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

            public void BeforeCallMethod(int calledParameters)
            {
            }
        }
    }
}