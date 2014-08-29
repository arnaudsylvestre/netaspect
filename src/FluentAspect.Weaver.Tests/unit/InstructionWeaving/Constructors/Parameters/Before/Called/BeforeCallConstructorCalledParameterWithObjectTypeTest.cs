using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Constructors.Parameters.Before.Called
{
    public class BeforeCallConstructorCalledParameterWithObjectTypeTest :
        NetAspectTest<BeforeCallConstructorCalledParameterWithObjectTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Called);
                    var classToWeave_L = ClassToWeave.Create();
                    Assert.AreEqual(classToWeave_L, MyAspect.Called);
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
            public static object Called;
            public bool NetAspectAttribute = true;

            public void BeforeCallConstructor(object called)
            {
                Called = called;
            }
        }
    }
}