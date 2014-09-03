using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Getter.Visibility
{
   public class CallGetPrivateFieldWeavingTest :
      NetAspectTest<CallGetPrivateFieldWeavingTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.Caller);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved();
            Assert.AreEqual(classToWeave_L, MyAspect.Caller);
         };
      }

      public class ClassToWeave
      {
         [MyAspect] private static string Field;

         public string Weaved()
         {
            return Field;
         }
      }

      public class MyAspect : Attribute
      {
         public static ClassToWeave Caller;
         public bool NetAspectAttribute = true;

         public void BeforeGetField(ClassToWeave caller)
         {
            Caller = caller;
         }
      }
   }
}
