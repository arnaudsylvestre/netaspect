using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Constructors.Parameters.Before.CallerParameterName
{
    public class BeforeCallConstructorCallerParameterNameReferencedParameterWithRealTypeTest :
        NetAspectTest<BeforeCallConstructorCallerParameterNameReferencedParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.ParameterName);
                    int val = 12;
                    var classToWeave_L = ClassToWeave.Create(ref val);
                    Assert.AreEqual(25, val);
                    Assert.AreEqual(12, MyAspect.ParameterName);
                };
        }
		
		
        public class ClassToWeave
        {
			[MyAspect]
            public ClassToWeave()
            {
            }

            public static ClassToWeave Create(ref int param1)
            {
                var res = new ClassToWeave();
                param1 = 25;
                return res;
            }
        }

        public class MyAspect : Attribute
        {
            public static int ParameterName;
            public bool NetAspectAttribute = true;

            public void BeforeCallConstructor(int callerParam1)
            {
                ParameterName = callerParam1;
            }
        }
    }
}