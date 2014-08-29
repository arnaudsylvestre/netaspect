using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Constructors.Parameters.Before.CalledParameters
{
    public class BeforeCallConstructorCalledParametersParameterWithRealTypeTest :
        NetAspectTest<BeforeCallConstructorCalledParametersParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.CalledParameters);
                    var classToWeave_L = ClassToWeave.Create();
                    Assert.AreEqual(new object[]
                        {
                            1, 2
                        }, MyAspect.CalledParameters);
                };
        }
		
        public class ClassToWeave
        {
			[MyAspect]
            public ClassToWeave(int param1, int param2)
            {
            }

            public static ClassToWeave Create()
            {
                return new ClassToWeave(1, 2);
            }
        }

        public class MyAspect : Attribute
        {
            public static object[] CalledParameters;
            public bool NetAspectAttribute = true;

            public void BeforeCallConstructor(object[] calledParameters)
            {
                CalledParameters = calledParameters;
            }
        }
    }
}