using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Getter.Parameters.After.FilePath
{
   public class AfterCallGetPropertyFilePathParameterWithRealTypeTest :
      NetAspectTest<AfterCallGetPropertyFilePathParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(null, MyAspect.FilePath);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved();
            Assert.True(MyAspect.FilePath.EndsWith(@"After\AfterCallGetPropertyFilePathParameterWithRealTypeTest.cs"));
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public string Property { get; set; }

         public string Weaved()
         {
            return Property;
         }
      }

      public class MyAspect : Attribute
      {
         public static string FilePath;
         public bool NetAspectAttribute = true;

         public void AfterGetProperty(string filePath)
         {
            FilePath = filePath;
         }
      }
   }
}
