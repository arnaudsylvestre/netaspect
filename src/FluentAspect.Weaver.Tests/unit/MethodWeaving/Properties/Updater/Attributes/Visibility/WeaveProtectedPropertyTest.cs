using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Attributes.Visibility
{
    public class WeaveProtectedPropertyTest : NetAspectTest<WeaveProtectedPropertyTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Instance);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.CallWeaved();
                    Assert.AreEqual(classToWeave_L, MyAspect.Instance);
                };
        }

        public class ClassToWeave
        {
            public void CallWeaved()
            {
                Weaved = "12";
            }

            [MyAspect]
            protected string Weaved
            {
               set { }
            }
        }

        public class MyAspect : Attribute
        {
            public static ClassToWeave Instance;
            public bool NetAspectAttribute = true;

            public void Before(ClassToWeave instance)
            {
                Instance = instance;
            }
        }
    }
}