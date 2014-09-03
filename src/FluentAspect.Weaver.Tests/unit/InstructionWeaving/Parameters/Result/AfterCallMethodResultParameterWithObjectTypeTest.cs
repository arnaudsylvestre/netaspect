using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.Result
{
   public class AfterCallMethodResultParameterWithObjectTypeTest :
      NetAspectTest<AfterCallMethodResultParameterWithObjectTypeTest.ClassToWeave>
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

         public void AfterCallMethod(object result)
         {
            Result = result.ToString();
         }
      }
   }
}
