using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.LifeCycle
{
   public class MethodWeavingWithPerInstanceLifeCycleWithExceptionTest :
      NetAspectTest<MethodWeavingWithPerInstanceLifeCycleWithExceptionTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            MyAspect.aspects = new List<MyAspect>();
            var classToWeave = new ClassToWeave();
            try { classToWeave.Weaved(); }
            catch (Exception) { }
            try { classToWeave.Weaved(); }
            catch (Exception) { }
            Assert.AreEqual(1, MyAspect.aspects.Count);
            classToWeave = new ClassToWeave();
            try { classToWeave.Weaved(); }
            catch (Exception) { }
            Assert.AreEqual(2, MyAspect.aspects.Count);
            Assert.AreEqual(2, MyAspect.aspects[0].nbBefore);
            Assert.AreEqual(2, MyAspect.aspects[0].nbExceptions);
            Assert.AreEqual(1, MyAspect.aspects[1].nbBefore);
            Assert.AreEqual(1, MyAspect.aspects[1].nbExceptions);
         };
      }


      public class ClassToWeave
      {
         [MyAspect]
         public void Weaved()
         {
             throw new Exception();
         }
      }

      public class MyAspect : Attribute
      {
         public static List<MyAspect> aspects = new List<MyAspect>();
         public static string LifeCycle = "PerInstance";
         public bool NetAspectAttribute = true;
         public int nbBefore = 0;
         public int nbExceptions = 0;

          public MyAspect()
         {
            aspects.Add(this);
         }


         public void BeforeMethod()
         {
             nbBefore++;
         }
         public void OnExceptionMethod()
         {
             nbExceptions++;
         }
      }
   }
}
