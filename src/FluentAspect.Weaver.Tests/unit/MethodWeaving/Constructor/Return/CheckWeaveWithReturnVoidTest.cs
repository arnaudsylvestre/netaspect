using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Return
{
    public class CheckWeaveWithReturnVoidTest : NetAspectTest<CheckWeaveWithReturnVoidTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Constructor);
                   new ClassToWeave();
                    Assert.AreEqual("Weaved", MyAspect.Constructor.Name);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave()
            {
            }
        }

        public class MyAspect : Attribute
        {
           public static ConstructorInfo Constructor;
            public bool NetAspectAttribute = true;

            public void BeforeConstructor(ConstructorInfo constructor)
            {
                Constructor = constructor;
            }
        }
    }
}