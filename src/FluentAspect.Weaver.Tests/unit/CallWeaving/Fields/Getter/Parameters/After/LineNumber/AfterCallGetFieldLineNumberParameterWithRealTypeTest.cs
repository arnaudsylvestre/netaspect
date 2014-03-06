using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.After.LineNumber
{
    public class AfterCallGetFieldLineNumberParameterWithRealTypeTest : NetAspectTest<AfterCallGetFieldLineNumberParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.AreEqual(0, MyAspect.LineNumber);
               var classToWeave_L = new ClassToWeave();
               classToWeave_L.Weaved();
               Assert.AreEqual(12, MyAspect.LineNumber);
            };
      }

      public class ClassToWeave
      {

          [MyAspect]
          public string Field;

         public string Weaved()
         {
             return Field;
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static int LineNumber;

         public void AfterGetField(int lineNumber)
         {
             LineNumber = lineNumber;
         }
      }
   }

   
}