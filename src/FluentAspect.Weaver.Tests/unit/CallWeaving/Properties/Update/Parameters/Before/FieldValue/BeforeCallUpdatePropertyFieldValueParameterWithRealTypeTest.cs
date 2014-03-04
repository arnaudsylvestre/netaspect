using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Properties.Update.Parameters.Before.FieldValue
{
    public class BeforeCallUpdatePropertyFieldValueParameterWithRealTypeTest : NetAspectTest<BeforeCallUpdatePropertyFieldValueParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.IsNull(MyAspect.FieldValue);
               var classToWeave_L = new ClassToWeave();
               classToWeave_L.Weaved();
               Assert.AreEqual(classToWeave_L, MyAspect.FieldValue);
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

         public static string FieldValue;

         public void BeforeUpdateProperty(string fieldValue)
         {
             FieldValue = fieldValue;
         }
      }
   }

   
}