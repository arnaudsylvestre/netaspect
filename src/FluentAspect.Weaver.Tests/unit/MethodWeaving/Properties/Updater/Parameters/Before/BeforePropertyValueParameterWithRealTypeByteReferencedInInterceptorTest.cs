using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.Before.Pdb
{
   public class BeforePropertyLineNumberParameterWithRealTypeByteReferencedInInterceptorTest :
      NetAspectTest<BeforePropertyLineNumberParameterWithRealTypeByteReferencedInInterceptorTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(0, MyAspect.LineNumber);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved = 12;
            Assert.AreEqual(25, MyAspect.LineNumber);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public byte Weaved
         {
            set { }
         }
      }

      public class MyAspect : Attribute
      {
         public static int LineNumber;
         public bool NetAspectAttribute = true;

         public void BeforePropertySetMethod(int lineNumber)
         {
            LineNumber = lineNumber;
         }
      }
   }
}
