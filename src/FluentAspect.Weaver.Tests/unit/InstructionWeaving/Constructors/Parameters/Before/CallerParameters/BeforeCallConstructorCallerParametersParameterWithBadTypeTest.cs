using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Constructors.Parameters.Before.CallerParameters
{
    public class BeforeCallConstructorCallerParametersParameterWithBadTypeTest :
        NetAspectTest<BeforeCallConstructorCallerParametersParameterWithBadTypeTest.ClassToWeave>
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
                        "the callerParameters parameter in the method BeforeCallConstructor of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.Object[]",
                        typeof(MyAspect).FullName, typeof(ClassToWeave).FullName)
                });
        }

        
        public class ClassToWeave
        {
			[MyAspect]
            public ClassToWeave()
            {
            }

            public static ClassToWeave Create()
            {
                return new ClassToWeave();
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void BeforeCallConstructor(int callerParameters)
            {
            }
        }
    }
}