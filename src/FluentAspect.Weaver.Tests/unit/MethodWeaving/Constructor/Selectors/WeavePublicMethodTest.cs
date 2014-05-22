using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Attributes.Visibility
{
   public class WeavePublicMethodWithSelectorTest : NetAspectTest<WeavePublicMethodWithSelectorTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Instance);
                    var classToWeave_L = new ClassToWeave();
                    Assert.AreEqual(classToWeave_L, MyAspect.Instance);
                };
        }

        public class ClassToWeave
        {
            public ClassToWeave()
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static ClassToWeave Instance;
            public bool NetAspectAttribute = true;

            public void BeforeConstructor(ClassToWeave instance)
            {
                Instance = instance;
            }



            public static bool SelectConstructor(ConstructorInfo constructor)
            {
               return constructor.DeclaringType.Name == "ClassToWeave";
            }
        }
    }
}