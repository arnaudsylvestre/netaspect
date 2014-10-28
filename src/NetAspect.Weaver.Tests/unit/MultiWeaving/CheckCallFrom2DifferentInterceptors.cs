using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MultiWeaving
{
   public class CheckCallFrom2DifferentInterceptors :
      NetAspectTest<CheckCallFrom2DifferentInterceptors.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect2.BeforeGetFieldInstance);
            Assert.IsNull(MyAspect.BeforeGetFieldInstance);
            var called = new ClassCalled();
            var classToWeave_L = new ClassToWeave(called);
            classToWeave_L.Weaved();
            Assert.AreEqual(called, MyAspect.BeforeGetFieldInstance);
            Assert.AreEqual(called, MyAspect2.BeforeGetFieldInstance);
         };
      }

      public class ClassCalled
      {
         [MyAspect] [MyAspect2] public string Field = "Value";
      }

      public class ClassToWeave
      {
         private readonly ClassCalled called;

         public ClassToWeave(ClassCalled called)
         {
            this.called = called;
         }

         [MyAspect]
         public string Weaved()
         {
            return called.Field;
         }
      }

      public class MyAspect : Attribute
      {
         public static ClassCalled BeforeGetFieldInstance;
         public bool NetAspectAttribute = true;

         public void BeforeGetField(ClassCalled instance)
         {
            BeforeGetFieldInstance = instance;
         }
      }

      public class MyAspect2 : Attribute
      {
         public static ClassCalled BeforeGetFieldInstance;
         public bool NetAspectAttribute = true;

         public void BeforeGetField(ClassCalled instance)
         {
            BeforeGetFieldInstance = instance;
         }
      }
   }
}
