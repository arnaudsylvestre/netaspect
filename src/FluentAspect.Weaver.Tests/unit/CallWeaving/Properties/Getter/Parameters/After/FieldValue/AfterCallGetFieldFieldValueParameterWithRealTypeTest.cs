using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.After.FieldValue
{
    public class AfterCallGetPropertyFieldValueParameterWithRealTypeTest : NetAspectTest<AfterCallGetPropertyFieldValueParameterWithRealTypeTest.ClassToWeave>
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

         public string Weaved()
         {
             return Field;
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static string FieldValue;

         public void AfterGetProperty(string fieldValue)
         {
             FieldValue = fieldValue;
         }
      }
   }

   
}