using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Constructors.Selectors
{
    public class AfterCallConstructorCalledParameterWithRealTypeAndSelectorTest :
        NetAspectTest<AfterCallConstructorCalledParameterWithRealTypeAndSelectorTest.ClassToWeave>
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
            public static ClassToWeave Called;
            public bool NetAspectAttribute = true;

            public void AfterCallConstructor(ClassToWeave called)
            {
                Called = called;
            }

            public static bool SelectConstructor(ConstructorInfo constructor)
            {
               return constructor.DeclaringType.Name == "ClassToWeave";
            }
        }
    }
}