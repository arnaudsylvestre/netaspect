using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.FileName
{
   public class AfterCallMethodFileNameParameterWithRealTypeTest :
      NetAspectTest<AfterCallMethodFileNameParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(null, MyAspect.FileName);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved();
            Assert.AreEqual("AfterCallMethodFileNameParameterWithRealTypeTest.cs", MyAspect.FileName);
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
         public static string FileName;
         public bool NetAspectAttribute = true;

         public void AfterCallMethod(string fileName)
         {
            FileName = fileName;
         }
      }
   }
}
