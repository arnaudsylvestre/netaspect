using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Methods.Parameters.Before.CallerParameterName
{
    public class BeforeCallMethodCallerParameterNameParameterWithBadTypeTest :
        NetAspectTest<BeforeCallMethodCallerParameterNameParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the callerParam1 parameter in the method BeforeCallMethod of the type '{0}' is declared with the type 'System.String' but it is expected to be System.Int32 because of the type of this parameter in the method Weaved of the type {1}",
                        typeof(MyAspect).FullName, typeof(ClassToWeave).FullName));
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string Method()
            {
                return "Hello";
            }

            public string Weaved(int param1)
            {
                return Method();
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void BeforeCallMethod(string callerParam1)
            {
            }
        }
    }
}