using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Update.Parameters.After.Caller
{
   public class AfterCallUpdateFieldCallerParameterWithRealTypeTest :
      NetAspectTest<AfterCallUpdateFieldCallerParameterWithRealTypeTest.ClassToWeave>
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
         [MyAspect] public string Field;

         public void Weaved()
         {
            Field = "Dummy";
         }
      }

      public class MyAspect : Attribute
      {
         public static ClassToWeave Caller;
         public bool NetAspectAttribute = true;

         public void AfterUpdateField(ClassToWeave caller)
         {
            Caller = caller;
         }
      }
   }
}
