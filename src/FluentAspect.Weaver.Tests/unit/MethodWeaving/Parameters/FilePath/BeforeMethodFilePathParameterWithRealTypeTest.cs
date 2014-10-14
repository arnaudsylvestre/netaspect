using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.FilePath
{
   public class BeforeMethodFilePathParameterWithRealTypeTest :
      NetAspectTest<BeforeMethodFilePathParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(null, MyAspect.FilePath);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved();
            Assert.True(MyAspect.FilePath.EndsWith(@"FilePath\BeforeMethodFilePathParameterWithRealTypeTest.cs"));
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
         public static string FilePath;
         public bool NetAspectAttribute = true;

         public void BeforeMethod(string filePath)
         {
            FilePath = filePath;
         }
      }
   }
}
