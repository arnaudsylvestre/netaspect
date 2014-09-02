using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Parameters.OnFinally.Instance
{
    public class OnFinallyPropertyInstanceParameterWithRealTypeTest :
        NetAspectTest<OnFinallyPropertyInstanceParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Instance);
                    var classToWeave_L = new ClassToWeave();
                    var value = classToWeave_L.Weaved;
                    Assert.AreEqual(classToWeave_L, MyAspect.Instance);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string Weaved
            {
                get { return "12"; }
            }
        }

        public class MyAspect : Attribute
        {
            public static ClassToWeave Instance;
            public bool NetAspectAttribute = true;

            public void OnFinallyPropertyGetMethod(ClassToWeave instance)
            {
                Instance = instance;
            }
        }
    }
}