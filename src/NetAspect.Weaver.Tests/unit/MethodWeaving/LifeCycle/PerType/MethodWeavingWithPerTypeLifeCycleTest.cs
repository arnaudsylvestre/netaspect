using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.LifeCycle
{
   public class MethodWeavingWithPerTypeLifeCycleTest :
      NetAspectTest<MethodWeavingWithPerTypeLifeCycleTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            MyAspect.aspects = new List<MyAspect>();
            var classToWeave = new ClassToWeave();
            classToWeave.Weaved();
            classToWeave = new ClassToWeave();
            classToWeave.Weaved();
            Assert.AreEqual(1, MyAspect.aspects.Count);
            Assert.AreEqual(2, MyAspect.aspects[0].nbAfter);
            Assert.AreEqual(2, MyAspect.aspects[0].nbBefore);
         };
      }


      public class ClassToWeave
      {
         [MyAspect]
         public void Weaved()
         {
         }
      }

      public class MyAspect : Attribute
      {
         public static List<MyAspect> aspects = new List<MyAspect>();
         public static string LifeCycle = "PerType";
         public bool NetAspectAttribute = true;
         public int nbAfter = 0;
         public int nbBefore = 0;

         public MyAspect()
         {
            aspects.Add(this);
         }

         public void AfterMethod()
         {
            nbAfter++;
         }

         public void BeforeMethod()
         {
            nbBefore++;
         }
      }
   }
}
