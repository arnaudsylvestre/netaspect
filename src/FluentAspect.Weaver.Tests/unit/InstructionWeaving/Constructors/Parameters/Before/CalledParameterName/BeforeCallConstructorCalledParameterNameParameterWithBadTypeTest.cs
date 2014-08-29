using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Constructors.Parameters.Before.CalledParameterName
{
    public class BeforeCallConstructorCalledParameterNameParameterWithBadTypeTest :
        NetAspectTest<BeforeCallConstructorCalledParameterNameParameterWithBadTypeTest.ClassToWeave>
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
                        "the calledParam1 parameter in the method BeforeCallConstructor of the type '{0}' is declared with the type 'System.String' but it is expected to be System.Int32",
                        typeof(MyAspect).FullName, typeof(ClassToWeave).FullName)
                });
        }

        public class ClassToWeave
        {
			[MyAspect]
            public ClassToWeave(int param1)
            {
            }

            public static ClassToWeave Create()
            {
                return new ClassToWeave(12);
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void BeforeCallConstructor(string calledParam1)
            {
            }
        }
    }
}