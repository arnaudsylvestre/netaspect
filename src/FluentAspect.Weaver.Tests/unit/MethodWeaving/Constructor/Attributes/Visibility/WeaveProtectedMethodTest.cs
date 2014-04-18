using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Attributes.Visibility
{
    public class WeaveProtectedMethodTest : NetAspectTest<WeaveProtectedMethodTest.ClassToWeave>
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
                Weaved();
            }

            [MyAspect]
           protected void Weaved()
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
        }
    }
}