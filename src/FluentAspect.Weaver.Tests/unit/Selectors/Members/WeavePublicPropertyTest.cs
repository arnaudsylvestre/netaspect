using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.Selectors.Members
{
   public class WeavePublicPropertyWithSelectorTest : NetAspectTest<WeavePublicPropertyWithSelectorTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.Instance);
            var classToWeave_L = new ClassToWeave();
            string value = classToWeave_L.Weaved;
            Assert.AreEqual(classToWeave_L, MyAspect.Instance);
         };
      }

      public class ClassToWeave
      {
         public string Weaved
         {
            get { return "12"; }
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


         public static bool SelectProperty(PropertyInfo property)
         {
            return property.Name == "Weaved";
         }
      }
   }
}
