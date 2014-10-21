using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.FileName
{
   public class AfterMethodFileNameParameterWithRealTypeTest :
      NetAspectTest<AfterMethodFileNameParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(null, MyAspect.FileName);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved();
            Assert.AreEqual("AfterMethodFileNameParameterWithRealTypeTest.cs", MyAspect.FileName);
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

         public void AfterMethod(string fileName)
         {
            FileName = fileName;
         }
      }
   }
}
