using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Parameters.Before.Instance
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
                    var value = classToWeave_L.WeavedForTest;
                    Assert.AreEqual(classToWeave_L, MyAspect.Instance);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string WeavedForTest
            {
				get {return "12";}
            }
        }

        public class MyAspect : Attribute
        {
            public static ClassToWeave Instance;
            public bool NetAspectAttribute = true;

            public void BeforePropertyGetMethod(ClassToWeave instance)
            {
                Instance = instance;
            }
        }
    }
}