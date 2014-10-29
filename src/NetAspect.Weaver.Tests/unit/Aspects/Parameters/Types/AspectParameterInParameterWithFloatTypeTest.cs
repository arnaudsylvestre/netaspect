using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.Aspects.Parameters.Types
{
   public class AspectParameterInParameterWithFloatTypeTest :
      NetAspectTest<AspectParameterInParameterWithFloatTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(0, MyAspect.I);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved(12);
            Assert.AreEqual(12, MyAspect.I);
            Assert.AreEqual(12.5f, MyAspect.Max);
         };
      }

      public class ClassToWeave
      {

          public void Weaved([MyAspect(12.5f)] int i)
         {
         }
      }

      public class MyAspect : Attribute
      {
          private readonly float _value;
          public static int I;
          public static float Max;
         public bool NetAspectAttribute = true;

          public MyAspect(float value)
          {
              _value = value;
          }

          public void AfterMethodForParameter(int parameterValue)
         {
            I = parameterValue;
            Max = _value;
         }
      }
   }
}
