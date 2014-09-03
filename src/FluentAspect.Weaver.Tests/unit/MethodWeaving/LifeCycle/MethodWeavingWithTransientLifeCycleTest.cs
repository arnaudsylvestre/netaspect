using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.LifeCycle
{
   public class MethodWeavingWithTransientLifeCycleTest :
      NetAspectTest<MethodWeavingWithTransientLifeCycleTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(0, MyAspect.i);
            var called = new ClassCalled();
            var classToWeave_L = new ClassToWeave(called);
            classToWeave_L.Weaved();
            Assert.AreEqual(1, MyAspect.i);
            classToWeave_L.Weaved();
            Assert.AreEqual(1, MyAspect.i);
         };
      }

      public class ClassCalled
      {
         [MyAspect] public string Field = "Value";
      }

      public class ClassToWeave
      {
         private readonly ClassCalled called;

         public ClassToWeave(ClassCalled called)
         {
            this.called = called;
         }

         public string Weaved()
         {
            return called.Field;
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
