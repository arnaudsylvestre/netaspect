using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Methods.Parameters.After.LineNumber
{
    public class AfterCallMethodLineNumberParameterWithRealTypeTest : NetAspectTest<AfterCallMethodLineNumberParameterWithRealTypeTest.ClassToWeave>
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
          public string Method() {return "Hello";}

         public string Weaved()
         {
             return Method();
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static int LineNumber;

         public void AfterCallMethod(int lineNumber)
         {
             LineNumber = lineNumber;
         }
      }
   }

   
}