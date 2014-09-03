using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.ParameterName
{
   public class AfterMethodParameterNameParameterWithRealTypeReferencedInInterceptorTest :
      NetAspectTest<AfterMethodParameterNameParameterWithRealTypeReferencedInInterceptorTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(0, MyAspect.I);
            var classToWeave_L = new ClassToWeave();
            Assert.AreEqual(12, classToWeave_L.Weaved(12));
            Assert.AreEqual(12, MyAspect.I);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public int Weaved(int i)
         {
            return i;
         }
      }

      public class MyAspect : Attribute
      {
         public static int I;
         public bool NetAspectAttribute = true;

         public void After(ref int i)
         {
            I = i;
            i = 3;
         }
      }
   }
}
