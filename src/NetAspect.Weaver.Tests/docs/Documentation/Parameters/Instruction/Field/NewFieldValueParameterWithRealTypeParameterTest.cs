using System;
using NUnit.Framework;
using NetAspect.Weaver.Tests.unit;

namespace NetAspect.Weaver.Tests.docs.Documentation.Parameters.Instruction.Field
{
   public class NewFieldValueParameterWithRealTypeParameterTest :
      NetAspectTest<NewFieldValueParameterWithRealTypeParameterTest.ClassToWeave>
   {
       public NewFieldValueParameterWithRealTypeParameterTest()
           : base("It must be declared with the same type as the field type", "GetFieldInstructionWeavingBefore", "InstructionFieldWeaving")
      {
      }

      protected override Action CreateEnsure()
      {
         return () =>
         {
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
