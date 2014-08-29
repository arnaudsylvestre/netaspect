using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Constructors.Parameters.Before.CallerParameterName
{
    public class BeforeCallConstructorCallerParameterNameParameterWithRealTypeOutTest :
        NetAspectTest<BeforeCallConstructorCallerParameterNameParameterWithRealTypeOutTest.ClassToWeave>
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
                        "impossible to out the parameter 'callerParam1' in the method BeforeCallConstructor of the type '{0}'",
                        typeof(MyAspect).FullName, typeof(ClassToWeave).FullName)
                });
        }		
		
        public class ClassToWeave
        {
			[MyAspect]
            public ClassToWeave()
            {
            }

            public static ClassToWeave Create(int param1)
            {
                return new ClassToWeave();
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void BeforeCallConstructor(out int callerParam1)
            {
                callerParam1 = 12;
            }
        }
    }
}