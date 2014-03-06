using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Methods.Parameters.After.FilePath
{
    public class AfterCallMethodFilePathParameterWithRealTypeTest : NetAspectTest<AfterCallMethodFilePathParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.AreEqual(null, MyAspect.FilePath);
               var classToWeave_L = new ClassToWeave();
               classToWeave_L.Weaved();
               Assert.AreEqual("", MyAspect.FilePath);
            };
      }

      public class ClassToWeave
      {

          [MyAspect]
          public string Method() {return "Hello";}

         public string Weaved()
         {
             return Method();
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static string FilePath;

         public void AfterCallMethod(string filePath)
         {
             FilePath = filePath;
         }
      }
   }

   
}