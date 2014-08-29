using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Constructors.Parameters.After.CallerParameterName
{
    public class AfterCallConstructorCallerParameterNameParameterWithObjectTypeTest :
        NetAspectTest<AfterCallConstructorCallerParameterNameParameterWithObjectTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(null, MyAspect.ParameterName);
                    var classToWeave_L = ClassToWeave.Create(12);
                    Assert.AreEqual(12, MyAspect.ParameterName);
                };
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
            public static object ParameterName;
            public bool NetAspectAttribute = true;

            public void AfterCallConstructor(object callerParam1)
            {
                ParameterName = callerParam1;
            }
        }
    }
}