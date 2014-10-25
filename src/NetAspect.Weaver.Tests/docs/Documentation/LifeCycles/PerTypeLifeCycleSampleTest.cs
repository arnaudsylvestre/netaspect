using System;
using NUnit.Framework;
using NetAspect.Weaver.Tests.unit;

namespace NetAspect.Weaver.Tests.docs.Documentation.LifeCycles
{
   public class PerTypeLifeCycleSampleTest :
      NetAspectTest<PerTypeLifeCycleSampleTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            var classToWeave = new ClassToWeave();
            classToWeave.Weaved();
            Assert.AreEqual(1, MyAspectAttribute.i);
            classToWeave.Weaved();
            Assert.AreEqual(2, MyAspectAttribute.i);
            var otherClassToWeave = new ClassToWeave();
            otherClassToWeave.Weaved();
            Assert.AreEqual(3, MyAspectAttribute.i);
         };
      }

      public class ClassToWeave
      {
          [MyAspect]
         public void Weaved()
         {
         }
      }

      public class MyAspectAttribute : Attribute
      {
         public static int i;

         public static string LifeCycle = "PerType";
         public bool NetAspectAttribute = true;

          public MyAspectAttribute()
          {
              i = 0;
          }

         public void BeforeMethod()
         {
            i++;
         }
      }
   }
}
