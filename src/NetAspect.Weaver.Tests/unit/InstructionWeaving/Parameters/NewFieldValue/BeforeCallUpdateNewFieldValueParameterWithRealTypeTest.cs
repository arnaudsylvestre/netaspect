using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.Value
{
   public class BeforeCallUpdateNewFieldValueParameterWithRealTypeTest :
      NetAspectTest<BeforeCallUpdateNewFieldValueParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(null, MyAspect.Value);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.field = "Hello";
            classToWeave_L.Weaved();
            Assert.AreEqual("Hello", MyAspect.Value);
         };
      }

      public class ClassToWeave
      {
         [MyAspect] public string field;

         public string Weaved()
         {
            field = "Hello";
            return field;
         }
      }

      public class MyAspect : Attribute
      {
         public static string Value;
         public bool NetAspectAttribute = true;

         public void BeforeUpdateField(string newFieldValue)
         {
             Value = newFieldValue;
         }
      }
   }
}
