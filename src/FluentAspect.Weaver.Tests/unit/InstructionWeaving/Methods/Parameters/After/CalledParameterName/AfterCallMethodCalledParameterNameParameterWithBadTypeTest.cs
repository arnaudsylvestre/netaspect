using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Methods.Parameters.After.CalledParameterName
{
    public class AfterCallMethodCalledParameterNameParameterWithBadTypeTest :
        NetAspectTest<AfterCallMethodCalledParameterNameParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Add(new ErrorReport.Error()
                {
                    Level = ErrorLevel.Error,
                    Message =
                    string.Format(
                        "the calledParam1 parameter in the method AfterCallMethod of the type '{0}' is declared with the type 'System.String' but it is expected to be System.Int32 because of the type of this parameter in the method Method of the type {1}",
                        typeof(MyAspect).FullName, typeof(ClassToWeave).FullName)
                });
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
            public bool NetAspectAttribute = true;

            public void AfterCallMethod(string calledParam1)
            {
            }
        }
    }
}