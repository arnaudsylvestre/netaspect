using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Update.Parameters.After.FileName
{
   public class AfterCallUpdatePropertyFileNameParameterWithRealTypeTest :
      NetAspectTest<AfterCallUpdatePropertyFileNameParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(null, MyAspect.FileName);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved();
            Assert.AreEqual("AfterCallUpdatePropertyFileNameParameterWithRealTypeTest.cs", MyAspect.FileName);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public string Property { get; set; }

         public void Weaved()
         {
            Property = "Dummy";
         }
      }

      public class MyAspect : Attribute
      {
         public static string FileName;
         public bool NetAspectAttribute = true;

         public void AfterUpdateProperty(string fileName)
         {
            FileName = fileName;
         }
      }
   }
}
