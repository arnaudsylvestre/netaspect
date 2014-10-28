using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MultiWeaving
{
   public class CheckWithIfCallTest :
      NetAspectTest<CheckWithIfCallTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            var called = new ClassCalled();
            var classToWeave_L = new ClassToWeave(called);
            Assert.AreEqual("Value", classToWeave_L.Weaved(true));
            Assert.AreEqual(called, MyAspect.BeforeGetFieldInstance);
            MyAspect.BeforeGetFieldInstance = null;
            Assert.AreEqual("", classToWeave_L.Weaved(false));
            Assert.Null(MyAspect.BeforeGetFieldInstance);
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

         [MyAspect]
         public string Weaved(bool accessField)
         {
             if (accessField)
                return called.Field;
             return "";
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
   }
}
