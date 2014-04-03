using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.After.Instance
{
    public class AfterPropertySetterInstanceParameterWithRealTypeTest :
        NetAspectTest<AfterPropertySetterInstanceParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Instance);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.MyProperty = "";
                    Assert.AreEqual(classToWeave_L, MyAspect.Instance);
                };
        }


        public class ClassToWeave
        {
            [MyAspect]
            public string MyProperty
            {
                set { }
            }
        }

        public class MyAspect : Attribute
        {
            public static ClassToWeave Instance;
            public bool NetAspectAttribute = true;

            public void AfterPropertySet(ClassToWeave instance)
            {
                Instance = instance;
            }
        }
    }
}