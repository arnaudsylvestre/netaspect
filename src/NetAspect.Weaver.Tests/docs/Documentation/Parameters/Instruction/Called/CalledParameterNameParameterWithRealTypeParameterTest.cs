using System;
using NUnit.Framework;
using NetAspect.Weaver.Tests.unit;

namespace NetAspect.Weaver.Tests.docs.Documentation.Parameters.Instruction.Called
{
   public class CalledParameterNameParameterWithRealTypeParameterTest : NetAspectTest<CalledParameterNameParameterWithRealTypeParameterTest.ClassToWeave>
   {

       public CalledParameterNameParameterWithRealTypeParameterTest()
           : base("It must be declared with the same type as the type of the parameter in the weaved method", "GetFieldInstructionWeavingBefore", "InstructionFieldWeaving")
      {
      }

      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(0, MyAspectAttribute.Value);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved();
            Assert.AreEqual(12, MyAspectAttribute.Value);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public string Method(int param1)
         {
            return "Hello";
         }

         public string Weaved()
         {
            return Method(12);
         }
      }

      public class MyAspectAttribute : Attribute
      {
         public static int Value;
         public bool NetAspectAttribute = true;

         public void BeforeCallMethod(int calledParam1)
         {
            Value = calledParam1;
         }
      }
   }
}
