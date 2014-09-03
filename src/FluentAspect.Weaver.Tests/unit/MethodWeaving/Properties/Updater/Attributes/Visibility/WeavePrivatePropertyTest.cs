using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Attributes.Visibility
{
   public class WeavePrivatePropertyTest : NetAspectTest<WeavePrivatePropertyTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.Instance);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.CallWeaved();
            Assert.AreEqual(classToWeave_L, MyAspect.Instance);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         private string Weaved
         {
            set { }
         }

         public void CallWeaved()
         {
            Weaved = "12";
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
      }
   }
}
