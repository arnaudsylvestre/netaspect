using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Constructors.Parameters.Before.CallerParameterName
{
    public class BeforeCallConstructorCallerParameterNameParameterWithRealTypeReferencedTest :
        NetAspectTest<BeforeCallConstructorCallerParameterNameParameterWithRealTypeReferencedTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.ParameterName);
                    Assert.AreEqual(10, ClassToWeave.Weaved(12));
                    Assert.AreEqual(12, MyAspect.ParameterName);
                };
        }
       
        public class ClassToWeave
        {
			[MyAspect]
            public ClassToWeave()
            {
            }

            public static int Weaved(int param1)
            {
                new ClassToWeave();
                return param1;
            }
        }

        public class MyAspect : Attribute
        {
            public static int ParameterName;
            public bool NetAspectAttribute = true;

            public void BeforeCallConstructor(ref int callerParam1)
            {
                ParameterName = callerParam1;
                callerParam1 = 10;
            }
        }
    }
}