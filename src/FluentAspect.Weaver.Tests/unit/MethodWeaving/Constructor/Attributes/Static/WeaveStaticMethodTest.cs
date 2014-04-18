using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Attributes.Static
{
    public class WeaveStaticMethodTest : NetAspectTest<WeaveStaticMethodTest.ClassToWeave>
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
            static ClassToWeave()
            {
            }
        }

        public class MyAspect : Attribute
        {
           public static MethodBase Constructor;
            public bool NetAspectAttribute = true;

            public void BeforeConstructor(MethodBase constructor)
            {
               Constructor = constructor;
            }
        }
    }
}