using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Constructors.Parameters.After.CalledParameterName
{
    public class AfterCallConstructorCalledParameterNameReferencedParameterWithRealTypeTest :
        NetAspectTest<AfterCallConstructorCalledParameterNameReferencedParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.ParameterName);
                    ClassToWeave.Create();
                    Assert.AreEqual(15, MyAspect.ParameterName);
                };
        }		
		
        public class ClassToWeave
        {
			[MyAspect]
            public ClassToWeave(ref int param1)
            {
                param1 = 15;
            }

            public static void Create()
            {
                int val = 12;
                new ClassToWeave(ref val);
                Assert.AreEqual(15, val);
            }
        }

        public class MyAspect : Attribute
        {
            public static int ParameterName;
            public bool NetAspectAttribute = true;

            public void AfterCallConstructor(int calledParam1)
            {
                ParameterName = calledParam1;
            }
        }
    }
}