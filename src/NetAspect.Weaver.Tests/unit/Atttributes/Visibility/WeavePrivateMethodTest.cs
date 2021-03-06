using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.Atttributes.Visibility
{
   public class WeavePrivateMethodTest : NetAspectTest<WeavePrivateMethodTest.ClassToWeave>
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
         public void CallWeaved()
         {
            Weaved();
         }

         [MyAspect]
         private void Weaved()
         {
         }
      }

      public class MyAspect : Attribute
      {
         public static ClassToWeave Instance;
         public bool NetAspectAttribute = true;

         public void BeforeMethod(ClassToWeave instance)
         {
            Instance = instance;
         }
      }
   }
}
