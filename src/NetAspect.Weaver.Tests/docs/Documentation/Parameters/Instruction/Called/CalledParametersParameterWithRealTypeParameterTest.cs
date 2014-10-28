using System;
using NUnit.Framework;
using NetAspect.Weaver.Tests.unit;

namespace NetAspect.Weaver.Tests.docs.Documentation.Parameters.Instruction.Called
{
   public class CalledParametersParameterWithRealTypeParameterTest :
      NetAspectTest<CalledParametersParameterWithRealTypeParameterTest.ClassToWeave>
   {
       public CalledParametersParameterWithRealTypeParameterTest()
           : base("It must be declared with the System.Object[] type", "GetFieldInstructionWeavingBefore", "InstructionFieldWeaving")
      {
      }
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspectAttribute.CalledParameters);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved();
            Assert.AreEqual(
               new object[]
               {
                  1, 2
               },
               MyAspectAttribute.CalledParameters);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public string Method(int param1, int param2)
         {
            return "Hello";
         }

         public string Weaved()
         {
            return Method(1, 2);
         }
      }

      public class MyAspectAttribute : Attribute
      {
         public static object[] CalledParameters;
         public bool NetAspectAttribute = true;

         public void BeforeCallMethod(object[] parameters)
         {
            CalledParameters = parameters;
         }
      }
   }
}
