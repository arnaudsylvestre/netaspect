using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Properties.Update.Parameters.After.FilePath
{
    public class AfterCallUpdatePropertyFilePathParameterWithRealTypeTest : NetAspectTest<AfterCallUpdatePropertyFilePathParameterWithRealTypeTest.ClassToWeave>
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
          public string Property {get;set;}

         public void Weaved()
         {
             Property = "Hello";
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static string FilePath;

         public void AfterUpdateProperty(string filePath)
         {
             FilePath = filePath;
         }
      }
   }

   
}