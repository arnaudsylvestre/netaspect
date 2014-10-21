using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.ParameterName
{
   public class AfterMethodParameterNameParameterWithRealTypeDoubleReferencedInInterceptorTest :
      NetAspectTest<AfterMethodParameterNameParameterWithRealTypeDoubleReferencedInInterceptorTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(0, MyAspect.I);
            var classToWeave_L = new ClassToWeave();
            double d = 2.3;
            classToWeave_L.Weaved(d);
            Assert.AreEqual(2.3, MyAspect.I);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public void Weaved(Double i)
         {
         }
      }

      public class MyAspect : Attribute
      {
         public static Double I;
         public bool NetAspectAttribute = true;

         public void AfterMethod(ref Double i)
         {
            I = i;
         }
      }
   }
}
