using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Getter.Parameters.After.LineNumber
{
   public class AfterCallGetPropertyLineNumberParameterWithRealTypeTest :
      NetAspectTest<AfterCallGetPropertyLineNumberParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(0, MyAspect.LineNumber);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved();
            Assert.AreEqual(27, MyAspect.LineNumber);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public string Property { get; set; }

         public string Weaved()
         {
            return Property;
         }
      }

      public class MyAspect : Attribute
      {
         public static int LineNumber;
         public bool NetAspectAttribute = true;

         public void AfterGetProperty(int lineNumber)
         {
            LineNumber = lineNumber;
         }
      }
   }
}
