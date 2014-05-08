using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Methods.Parameters.Before.CalledParameterName
{
    public class BeforeCallMethodCalledParameterNameParameterWithRealTypeReferencedTest :
        NetAspectTest<BeforeCallMethodCalledParameterNameParameterWithRealTypeReferencedTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "impossible to ref/out the parameter 'calledParam1' in the method BeforeCallMethod of the type '{0}'",
                        typeof(MyAspect).FullName));
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string Method(int param1)
            {
                return "Hello";
            }

            public string Weaved()
            {
                return Method(12);
            }
        }

        public class MyAspect : Attribute
        {
            public static int ParameterName;
            public bool NetAspectAttribute = true;

            public void BeforeCallMethod(ref int calledParam1)
            {
                ParameterName = calledParam1;
            }
        }
    }
}