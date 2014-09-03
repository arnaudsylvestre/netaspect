using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Methods.Parameters.After.Result
{
   public class AfterCallMethodResultParameterWithRealTypeTest :
      NetAspectTest<AfterCallMethodResultParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(null, MyAspect.Result);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved();
            Assert.AreEqual("Hello", MyAspect.Result);
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

      public class MyAspect : Attribute
      {
         public static string Result;
         public bool NetAspectAttribute = true;

         public void AfterCallMethod(string result)
         {
            Result = result;
         }
      }
   }
}
