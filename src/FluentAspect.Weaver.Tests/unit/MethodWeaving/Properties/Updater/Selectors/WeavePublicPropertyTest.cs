using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Attributes.Visibility
{
   public class WeavePublicPropertyWithSelectorTest : NetAspectTest<WeavePublicPropertyWithSelectorTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Instance);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved = "12";
                    Assert.AreEqual(classToWeave_L, MyAspect.Instance);
                };
        }

        public class ClassToWeave
        {
            public string Weaved
           {
              set { }
            }
        }

        public class MyAspect : Attribute
        {
            public static ClassToWeave Instance;
            public bool NetAspectAttribute = true;

            public void BeforePropertySetMethod(ClassToWeave instance)
            {
                Instance = instance;
            }


            public static bool SelectProperty(PropertyInfo property)
            {
               return property.Name == "Weaved";
            }
        }
    }
}