using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.LifeCycle
{
   public class MethodWeavingWithTransientLifeCycleInStaticTest :
      NetAspectTest<MethodWeavingWithTransientLifeCycleInStaticTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(0, MyAspect.i);
            ClassToWeave.Weaved();
            Assert.AreEqual(1, MyAspect.i);
            ClassToWeave.Weaved();
            Assert.AreEqual(1, MyAspect.i);
         };
      }

      public class ClassCalled
      {
         [MyAspect] public static string Field = "Value";
      }

      public class ClassToWeave
      {

         public static string Weaved()
         {
             return ClassCalled.Field;
         }
      }

      public class MyAspect : Attribute
      {
         public static int i = 0;

         public static string LifeCycle = "Transient";
         public bool NetAspectAttribute = true;

         public MyAspect()
         {
            i = 0;
         }

         public void AfterGetField()
         {
            i++;
         }
      }
   }
}
