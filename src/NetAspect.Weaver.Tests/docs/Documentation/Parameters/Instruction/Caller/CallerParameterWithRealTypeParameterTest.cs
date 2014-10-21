using System;
using NUnit.Framework;
using NetAspect.Weaver.Tests.unit;

namespace NetAspect.Weaver.Tests.docs.Documentation.Parameters.Instruction.Caller
{
   public class CallerParameterWithRealTypeParameterTest :
      NetAspectTest<CallerParameterWithRealTypeParameterTest.ClassToWeave>
   {
       public CallerParameterWithRealTypeParameterTest()
           : base("It can be declared with the real type", "GetFieldInstructionWeavingBefore", "InstructionFieldWeaving")
      {
      }
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspectAttribute.Caller);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved();
            Assert.AreEqual(classToWeave_L, MyAspectAttribute.Caller);
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
         public static ClassToWeave Caller;
         public bool NetAspectAttribute = true;

         public void BeforeCallMethod(ClassToWeave caller)
         {
            Caller = caller;
         }
      }
   }
}
