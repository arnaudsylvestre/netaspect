using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Attributes.Visibility
{
   public class WeaveProtectedPropertyTest : NetAspectTest<WeaveProtectedPropertyTest.ClassToWeave>
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
         protected string Weaved
         {
            get { return "12"; }
         }

         public void CallWeaved()
         {
            string value = Weaved;
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
      }
   }
}
