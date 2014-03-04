using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Methods.Parameters.Before.ColumnNumber
{
    public class BeforeCallMethodColumnNumberParameterWithRealTypeTest : NetAspectTest<BeforeCallMethodColumnNumberParameterWithRealTypeTest.ClassToWeave>
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
          public string Method() {return "Hello";}

         public string Weaved()
         {
             return Method();
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static int ColumnNumber;

         public void BeforeCallMethod(int columnNumber)
         {
             ColumnNumber = columnNumber;
         }
      }
   }

   
}