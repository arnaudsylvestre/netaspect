using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.Aspects.Parameters
{
   public class AfterMethodParameterNameParameterWithRealTypeTest :
      NetAspectTest<AfterMethodParameterNameParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(0, MyAspect.I);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved(12);
            Assert.AreEqual(12, MyAspect.I);
            Assert.AreEqual(3, MyAspect.Max);
         };
      }

      public class ClassToWeave
      {
         [MyAspect(3)]
         public void Weaved(int i)
         {
         }
      }

      public class MyAspect : Attribute
      {
          private readonly int _max;
          public static int I;
          public static int Max;
         public bool NetAspectAttribute = true;

          public MyAspect(int max)
          {
              _max = max;
          }

          public void AfterMethod(int i)
         {
            I = i;
              Max = _max;
         }
      }
   }
}
