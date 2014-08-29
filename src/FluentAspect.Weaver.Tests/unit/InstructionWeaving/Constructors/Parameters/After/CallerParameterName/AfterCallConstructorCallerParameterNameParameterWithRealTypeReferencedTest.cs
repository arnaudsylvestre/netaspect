using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Constructors.Parameters.After.CallerParameterName
{
    public class AfterCallConstructorCallerParameterNameParameterWithRealTypeReferencedTest :
        NetAspectTest<AfterCallConstructorCallerParameterNameParameterWithRealTypeReferencedTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.ParameterName);
                    Assert.AreEqual(10, ClassToWeave.Create(12));
                    Assert.AreEqual(12, MyAspect.ParameterName);
                };
        }
		
        public class ClassToWeave
        {
			[MyAspect]
            public ClassToWeave()
            {
            }

            public static int Create(int param1)
            {
                new ClassToWeave();
                return param1;
            }
        }

        public class MyAspect : Attribute
        {
            public static int ParameterName;
            public bool NetAspectAttribute = true;

            public void AfterCallConstructor(ref int callerParam1)
            {
                ParameterName = callerParam1;
                callerParam1 = 10;
            }
        }
    }
}