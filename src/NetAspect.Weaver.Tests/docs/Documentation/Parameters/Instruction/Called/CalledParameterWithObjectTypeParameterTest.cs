using System;
using NUnit.Framework;
using NetAspect.Weaver.Tests.unit;

namespace NetAspect.Weaver.Tests.docs.Documentation.Parameters.Instruction.Called
{
   public class CalledParameterWithObjectTypeParameterTest :
      NetAspectTest<CalledParameterWithObjectTypeParameterTest.ClassToWeave>
   {


       public CalledParameterWithObjectTypeParameterTest()
           : base("It can be declared with the System.Object type", "GetFieldInstructionWeavingBefore", "InstructionFieldWeaving")
      {
      }
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspectAttribute.Called);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved();
            Assert.AreEqual(classToWeave_L, MyAspectAttribute.Called);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public string Method()
         {
            return "Hello";
         }

         public string Weaved()
         {
            return Method();
         }
      }

      public class MyAspectAttribute : Attribute
      {
          public static object Called;
         public bool NetAspectAttribute = true;

         public void BeforeCallMethod(object called)
         {
            Called = called;
         }
      }
   }
}