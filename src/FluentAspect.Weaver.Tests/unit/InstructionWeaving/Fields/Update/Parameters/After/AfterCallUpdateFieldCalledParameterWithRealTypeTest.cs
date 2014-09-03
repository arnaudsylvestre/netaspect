using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Update.Parameters.After.Called
{
   public class AfterCallUpdateFieldCalledParameterWithRealTypeTest :
      NetAspectTest<AfterCallUpdateFieldCalledParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.Called);
            var called = new ClassCalled();
            var classToWeave_L = new ClassToWeave(called);
            classToWeave_L.Weaved();
            Assert.AreEqual(called, MyAspect.Called);
            Assert.AreEqual("Dummy", called.Field);
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

         public void Weaved()
         {
            called.Field = "Dummy";
         }
      }

      public class MyAspect : Attribute
      {
         public static ClassCalled Called;
         public bool NetAspectAttribute = true;

         public void AfterUpdateField(ClassCalled called)
         {
            Called = called;
         }
      }
   }
}
