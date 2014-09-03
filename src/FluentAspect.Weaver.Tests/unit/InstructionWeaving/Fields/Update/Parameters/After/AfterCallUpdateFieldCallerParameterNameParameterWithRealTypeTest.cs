using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Update.Parameters.After.CallerParameterName
{
   public class AfterCallUpdateFieldCallerParameterNameParameterWithRealTypeTest :
      NetAspectTest<AfterCallUpdateFieldCallerParameterNameParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(0, MyAspect.ParameterName);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved(12);
            Assert.AreEqual(12, MyAspect.ParameterName);
         };
      }

      public class ClassToWeave
      {
         [MyAspect] public string Field;

         public void Weaved(int param1)
         {
            Field = "Dummy";
         }
      }

      public class MyAspect : Attribute
      {
         public static int ParameterName;
         public bool NetAspectAttribute = true;

         public void AfterUpdateField(int callerParam1)
         {
            ParameterName = callerParam1;
         }
      }
   }
}
