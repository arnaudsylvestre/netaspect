using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Constructors.Parameters.Before.CallerParameters
{
    public class BeforeCallConstructorCallerParametersParameterWithRealTypeTest :
        NetAspectTest<BeforeCallConstructorCallerParametersParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.CallerParameters);
                    var classToWeave_L = ClassToWeave.Create(1, 2);
                    Assert.AreEqual(new object[]
                        {
                            1, 2
                        }, MyAspect.CallerParameters);
                };
        }

		
        public class ClassToWeave
        {
			[MyAspect]
            public ClassToWeave()
            {
            }

            public static ClassToWeave Create(int param1, int param2)
            {
                return new ClassToWeave();
            }
        }

        public class MyAspect : Attribute
        {
            public static object[] CallerParameters;
            public bool NetAspectAttribute = true;

            public void BeforeCallConstructor(object[] callerParameters)
            {
                CallerParameters = callerParameters;
            }
        }
    }
}