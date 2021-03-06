using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.ParameterName
{
   public class AfterMethodParameterNameParameterWithRealTypeReferencedTest :
      NetAspectTest<AfterMethodParameterNameParameterWithRealTypeReferencedTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(0, MyAspect.I);
            var classToWeave_L = new ClassToWeave();
            int value = 12;
            Assert.AreEqual(8, classToWeave_L.Weaved(ref value));
            Assert.AreEqual(8, MyAspect.I);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public int Weaved(ref int i)
         {
            i = 8;
            return i;
         }
      }

      public class MyAspect : Attribute
      {
         public static int I;
         public bool NetAspectAttribute = true;

         public void AfterMethod(int i)
         {
            I = i;
         }
      }
   }
}
