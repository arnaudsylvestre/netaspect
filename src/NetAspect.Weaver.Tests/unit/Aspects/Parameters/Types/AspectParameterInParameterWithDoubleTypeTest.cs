using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.Aspects.Parameters.Types
{
   public class AspectParameterInParameterWithDoubleTypeTest :
      NetAspectTest<AspectParameterInParameterWithDoubleTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(0, MyAspect.I);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved(12);
            Assert.AreEqual(12, MyAspect.I);
            Assert.AreEqual(12.5, MyAspect.Max);
         };
      }

      public class ClassToWeave
      {

          public void Weaved([MyAspect(12.5)] int i)
         {
         }
      }

      public class MyAspect : Attribute
      {
          private readonly double _value;
          public static int I;
          public static double Max;
         public bool NetAspectAttribute = true;

          public MyAspect(double value)
          {
              _value = value;
          }

          public void AfterMethodForParameter(int i)
         {
            I = i;
            Max = _value;
         }
      }
   }
}
