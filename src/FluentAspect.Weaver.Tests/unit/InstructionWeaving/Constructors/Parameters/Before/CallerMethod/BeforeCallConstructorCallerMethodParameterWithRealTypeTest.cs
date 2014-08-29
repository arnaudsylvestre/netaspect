using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Constructors.Parameters.Before.CallerMethod
{
    public class BeforeCallConstructorCallerMethodParameterWithRealTypeTest :
        NetAspectTest<BeforeCallConstructorCallerMethodParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(null, MyAspect.Method);
                    ClassToWeave.Create();
                    Assert.AreEqual("Weaved", MyAspect.Method.Name);
                };
        }
		
        public class ClassToWeave
        {
			[MyAspect]
            public ClassToWeave()
            {
            }

            public static ClassToWeave Create()
            {
                return new ClassToWeave();
            }
        }

        public class MyAspect : Attribute
        {
            public static MethodBase Method;
            public bool NetAspectAttribute = true;

            public void BeforeCallConstructor(MethodBase callerMethod)
            {
                Method = callerMethod;
            }
        }
    }
}