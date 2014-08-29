using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Constructors.Parameters.After.CalledParameterName
{
    public class AfterCallConstructorCalledParameterNameParameterWithRealTypeReferencedTest :
        NetAspectTest<AfterCallConstructorCalledParameterNameParameterWithRealTypeReferencedTest.ClassToWeave>
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
                        "impossible to ref/out the parameter 'calledParam1' in the method AfterCallConstructor of the type '{0}'",
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
            public static int ParameterName;
            public bool NetAspectAttribute = true;

            public void AfterCallConstructor(ref int calledParam1)
            {
                ParameterName = calledParam1;
            }
        }
    }
}