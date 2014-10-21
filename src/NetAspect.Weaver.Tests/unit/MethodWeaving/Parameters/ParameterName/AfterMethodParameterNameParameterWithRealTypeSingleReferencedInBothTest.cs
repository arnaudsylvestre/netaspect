using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.ParameterName
{
   public class AfterMethodParameterNameParameterWithRealTypeSingleReferencedInBothTest :
      NetAspectTest<AfterMethodParameterNameParameterWithRealTypeSingleReferencedInBothTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(0, MyAspect.I);
            var classToWeave_L = new ClassToWeave();
            float f = 3F;
            classToWeave_L.Weaved(ref f);
            Assert.AreEqual(3F, MyAspect.I);
            Assert.AreEqual(6F, f);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public void Weaved(ref Single i)
         {
         }
      }

      public class MyAspect : Attribute
      {
         public static Single I;
         public bool NetAspectAttribute = true;

         public void AfterMethod(ref Single i)
         {
            I = i;
            i = 6;
         }
      }
   }
}
