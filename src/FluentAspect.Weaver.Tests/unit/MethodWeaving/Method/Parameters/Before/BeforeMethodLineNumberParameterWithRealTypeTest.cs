using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.Before.LineNumber
{
   public class BeforeMethodLineNumberParameterWithRealTypeTest :
      NetAspectTest<BeforeMethodLineNumberParameterWithRealTypeTest.ClassToWeave>
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
         public void Weaved()
         {
         }
      }

      public class MyAspect : Attribute
      {
         public static int LineNumber;
         public bool NetAspectAttribute = true;

         public void Before(int lineNumber)
         {
            LineNumber = lineNumber;
         }
      }
   }
}
