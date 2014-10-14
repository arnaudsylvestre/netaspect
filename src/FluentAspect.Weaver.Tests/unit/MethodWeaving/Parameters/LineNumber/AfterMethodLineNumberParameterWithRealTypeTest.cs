using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.LineNumber
{
   public class AfterMethodLineNumberParameterWithRealTypeTest :
      NetAspectTest<AfterMethodLineNumberParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(0, MyAspect.LineNumber);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved();
            Assert.AreEqual(24, MyAspect.LineNumber);
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
         public static int LineNumber;
         public bool NetAspectAttribute = true;

         public void AfterMethod(int lineNumber)
         {
            LineNumber = lineNumber;
         }
      }
   }
}
