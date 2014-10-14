using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.ColumnNumber
{
   public class AfterMethodColumnNumberParameterWithRealTypeTest :
      NetAspectTest<AfterMethodColumnNumberParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(0, MyAspect.ColumnNumber);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved();
            Assert.AreEqual(13, MyAspect.ColumnNumber);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public string Method()
         {
            return "Hello";
         }

         public string Weaved()
         {
            return Method();
         }
      }

      public class MyAspect : Attribute
      {
         public static int ColumnNumber;
         public bool NetAspectAttribute = true;

         public void AfterMethod(int columnNumber)
         {
            ColumnNumber = columnNumber;
         }
      }
   }
}
