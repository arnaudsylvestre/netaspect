using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.After.ParameterName
{
    public class AfterPropertyParameterNameParameterWithObjectTypeTest :
        NetAspectTest<AfterPropertyParameterNameParameterWithObjectTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(null, MyAspect.I);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved = "12";
                    Assert.AreEqual(12, MyAspect.I);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string Weaved
           {
               set { }
            }
        }

        public class MyAspect : Attribute
        {
            public static object I;
            public bool NetAspectAttribute = true;

            public void After(object i)
            {
                I = i;
            }
        }
    }
}