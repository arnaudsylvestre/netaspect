using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Properties.Update.Parameters.Before.ColumnNumber
{
    public class BeforeCallUpdatePropertyColumnNumberParameterWithRealTypeTest : NetAspectTest<BeforeCallUpdatePropertyColumnNumberParameterWithRealTypeTest.ClassToWeave>
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

         public void Weaved()
         {
             Property = "Hello";
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static int ColumnNumber;

         public void BeforeUpdateProperty(int columnNumber)
         {
             ColumnNumber = columnNumber;
         }
      }
   }

   
}