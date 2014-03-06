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
                Assert.AreEqual("NeverUsedValue", MyAspect.Result);
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

            public static string Result;

            public void After(ref string result)
            {
                Result = result;
                result = "MyNewValue";
            }
        }
    }

   
}