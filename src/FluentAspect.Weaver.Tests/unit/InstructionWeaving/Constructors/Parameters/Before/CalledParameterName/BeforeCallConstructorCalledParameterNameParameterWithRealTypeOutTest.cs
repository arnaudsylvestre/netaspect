using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Constructors.Parameters.Before.CalledParameterName
{
    public class BeforeCallConstructorCalledParameterNameParameterWithRealTypeOutTest :
        NetAspectTest<BeforeCallConstructorCalledParameterNameParameterWithRealTypeOutTest.ClassToWeave>
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
                        "impossible to ref/out the parameter 'calledParam1' in the method BeforeCallConstructor of the type '{0}'",
                        typeof(MyAspect).FullName)
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

            public void BeforeCallConstructor(out int calledParam1)
            {
                calledParam1 = 12;
            }
        }
    }
}