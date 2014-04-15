using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Attributes.Visibility
{
    public class WeavePrivatePropertyTest : NetAspectTest<WeavePrivatePropertyTest.ClassToWeave>
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
                var value = Weaved;
            }

            [MyAspect]
            private string Weaved
            {
				get {return "12";}
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