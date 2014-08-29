using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Constructors.Parameters.Before.Caller
{
    public class BeforeCallConstructorCallerParameterWithObjectTypeTest :
        NetAspectTest<BeforeCallConstructorCallerParameterWithObjectTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Caller);
                    var classToWeave = new ClassToWeave();
                    classToWeave.Create();
                    Assert.AreSame(classToWeave, MyAspect.Caller);
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

            public void BeforeCallConstructor(object caller)
            {
                Caller = caller;
            }
        }
    }
}