using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Constructors.Parameters.After.Caller
{
    public class AfterCallConstructorCallerParameterWithObjectTypeTest :
        NetAspectTest<AfterCallConstructorCallerParameterWithObjectTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Caller);
                    var classToWeave = new ClassToWeave();
                    var classToWeave_L = classToWeave.Create();
                    Assert.AreEqual(classToWeave, MyAspect.Caller);
                };
        }


        public class ClassToWeave
        {
			[MyAspect]
            public ClassToWeave()
            {
            }

            public ClassToWeave Create()
            {
                return new ClassToWeave();
            }
        }

        public class MyAspect : Attribute
        {
            public static object Caller;
            public bool NetAspectAttribute = true;

            public void AfterCallConstructor(object caller)
            {
                Caller = caller;
            }
        }
    }
}