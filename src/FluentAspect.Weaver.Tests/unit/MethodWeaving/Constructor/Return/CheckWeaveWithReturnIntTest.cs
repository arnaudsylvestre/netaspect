using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Return
{
    public class CheckWeaveWithReturnIntTest : NetAspectTest<CheckWeaveWithReturnIntTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Constructor);
                    int res = new ClassToWeave().Weaved();
                    Assert.AreEqual("Weaved", MyAspect.Constructor.Name);
                    Assert.AreEqual(12, res);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public int Weaved()
            {
                return 12;
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