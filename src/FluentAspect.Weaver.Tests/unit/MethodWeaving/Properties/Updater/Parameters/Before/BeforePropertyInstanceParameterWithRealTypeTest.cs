using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.Before.Instance
{
    public class BeforePropertyInstanceParameterWithRealTypeTest :
        NetAspectTest<BeforePropertyInstanceParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Instance);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.WeavedForTest = 12;
                    Assert.AreEqual(classToWeave_L, MyAspect.Instance);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public int WeavedForTest
            {
                set { }
            }
        }

        public class MyAspect : Attribute
        {
            public static ClassToWeave Instance;
            public bool NetAspectAttribute = true;

            public void BeforePropertySetMethod(ClassToWeave instance)
            {
                Instance = instance;
            }
        }
    }
}