using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.After.Result
{
    public class AfterPropertyResultParameterWithObjectTypeTest :
        NetAspectTest<AfterPropertyResultParameterWithObjectTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Result);
                    var classToWeave_L = new ClassToWeave();
                    string res = classToWeave_L.Weaved();
                    Assert.AreEqual(res, MyAspect.Result);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string Weaved()
            {
                return "Hello";
            }
        }

        public class MyAspect : Attribute
        {
            public static object Result;
            public bool NetAspectAttribute = true;

            public void After(object result)
            {
                Result = result;
            }
        }
    }
}