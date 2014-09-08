using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.ParameterName
{
   public class AfterMethodParameterNameParameterWithRealTypeShortTest :
      NetAspectTest<AfterMethodParameterNameParameterWithRealTypeShortTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(0, MyAspect.I);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved(12);
            Assert.AreEqual(12, MyAspect.I);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public void Weaved(short i)
         {
         }
      }

      public class MyAspect : Attribute
      {
         public static short I;
         public bool NetAspectAttribute = true;

         public void AfterMethod(short i)
         {
            I = i;
         }
      }
   }
}
