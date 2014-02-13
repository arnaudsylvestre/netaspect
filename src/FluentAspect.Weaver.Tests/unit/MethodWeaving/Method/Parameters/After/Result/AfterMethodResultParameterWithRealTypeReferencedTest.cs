using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Result
{
    public class AfterMethodResultParameterWithRealTypeReferencedTest : NetAspectTest<AfterMethodResultParameterWithRealTypeReferencedTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
            {
                var classToWeave_L = new ClassToWeave();
                var res = classToWeave_L.Weaved();
                Assert.AreEqual("MyNewValue", res);
                Assert.AreEqual("NeverUsedValue", MyAspect.Instance);
            };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string Weaved()
            {
                return "NeverUsedValue";
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public static string Instance;

            public void After(ref string instance)
            {
                Instance = instance;
                instance = "MyNewValue";
            }
        }
    }

   
}