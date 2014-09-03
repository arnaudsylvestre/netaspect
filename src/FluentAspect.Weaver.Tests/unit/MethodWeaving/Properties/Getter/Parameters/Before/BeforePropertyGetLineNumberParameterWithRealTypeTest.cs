using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Parameters.Before.Pdb
{
   public class BeforePropertyGetLineNumberParameterWithRealTypeTest :
      NetAspectTest<BeforePropertyGetLineNumberParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(0, MyAspect.LineNumber);
            var classToWeave_L = new ClassToWeave();
            string value = classToWeave_L.Weaved;
            Assert.AreEqual(25, MyAspect.LineNumber);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public string Weaved
         {
            get { return "12"; }
         }
      }

      public class MyAspect : Attribute
      {
         public static int LineNumber;
         public bool NetAspectAttribute = true;

         public void BeforePropertyGetMethod(int lineNumber)
         {
            LineNumber = lineNumber;
         }
      }
   }
}
