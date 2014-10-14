using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.FileName
{
   public class BeforeMethodFileNameParameterWithRealTypeTest :
      NetAspectTest<BeforeMethodFileNameParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(null, MyAspect.FileName);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved();
            Assert.AreEqual("BeforeMethodFileNameParameterWithRealTypeTest.cs", MyAspect.FileName);
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

         public void BeforeMethod(string fileName)
         {
            FileName = fileName;
         }
      }
   }
}
