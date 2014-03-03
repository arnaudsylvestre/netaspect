using System;
using System.Reflection;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.After.Field
{
    public class AfterCallUpdatePropertyFieldParameterWithRealTypeTest : NetAspectTest<AfterCallUpdatePropertyFieldParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.AreEqual(null, MyAspect.Field);
               var classToWeave_L = new ClassToWeave();
               classToWeave_L.Weaved();
               Assert.AreEqual("Field", MyAspect.Field.Name);
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

         public static FieldInfo Field;

         public void AfterUpdateProperty(FieldInfo columnNumber)
         {
             Field = columnNumber;
         }
      }
   }

   
}