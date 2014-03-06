using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Properties.Getter.Parameters.After.ColumnNumber
{
    public class AfterCallGetPropertyColumnNumberParameterWithRealTypeTest : NetAspectTest<AfterCallGetPropertyColumnNumberParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.AreEqual(0, MyAspect.ColumnNumber);
               var classToWeave_L = new ClassToWeave();
               classToWeave_L.Weaved();
               Assert.AreEqual(12, MyAspect.ColumnNumber);
            };
      }

      public class ClassToWeave
      {

          [MyAspect]
          public string Property {get;set;}

         public string Weaved()
         {
             return Property;
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static int ColumnNumber;

         public void AfterGetProperty(int columnNumber)
         {
             ColumnNumber = columnNumber;
         }
      }
   }

   
}