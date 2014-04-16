using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.After.Result
{
    public class AfterPropertyResultParameterWithRealTypeReferencedTest :
        NetAspectTest<AfterPropertyResultParameterWithRealTypeReferencedTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    var classToWeave_L = new ClassToWeave();
                    string res = classToWeave_L.Weaved();
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
            public static string Result;
            public bool NetAspectAttribute = true;

            public void After(ref string result)
            {
                Result = result;
                result = "MyNewValue";
            }
        }
    }
}