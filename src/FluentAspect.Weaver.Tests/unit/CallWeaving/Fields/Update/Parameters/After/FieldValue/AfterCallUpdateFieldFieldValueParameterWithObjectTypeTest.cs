using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Update.Parameters.After.FieldValue
{
    public class AfterCallUpdateFieldFieldValueParameterWithObjectTypeTest : NetAspectTest<AfterCallUpdateFieldFieldValueParameterWithObjectTypeTest.ClassToWeave>
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
          public string Field;

         public void Weaved()
         {
             Field = "Hello";
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static object FieldValue;

         public void AfterUpdateField(object fieldValue)
         {
             FieldValue = fieldValue;
         }
      }
   }

   
}