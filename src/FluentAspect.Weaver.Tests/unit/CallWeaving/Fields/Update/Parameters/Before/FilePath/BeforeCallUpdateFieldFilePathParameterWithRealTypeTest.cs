using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Update.Parameters.Before.FilePath
{
    public class BeforeCallUpdateFieldFilePathParameterWithRealTypeTest : NetAspectTest<BeforeCallUpdateFieldFilePathParameterWithRealTypeTest.ClassToWeave>
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
          public string Field;

         public void Weaved()
         {
             Field = "Hello";
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static string FilePath;

         public void BeforeUpdateField(string filePath)
         {
             FilePath = filePath;
         }
      }
   }

   
}