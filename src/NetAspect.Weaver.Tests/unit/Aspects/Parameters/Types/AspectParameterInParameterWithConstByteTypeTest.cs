using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.Aspects.Parameters.Types
{
   public class AspectParameterInParameterWithConstByteTypeTest :
      NetAspectTest<AspectParameterInParameterWithConstByteTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(0, MyAspect.I);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved(12);
            Assert.AreEqual(12, MyAspect.I);
            Assert.AreEqual(5, MyAspect.Max);
         };
      }

      public class ClassToWeave
      {
          private const byte FIVE = 5;
          public void Weaved([MyAspect(FIVE)] int i)
         {
         }
      }

      public class MyAspect : Attribute
      {
          private readonly byte _value;
          public static int I;
          public static byte Max;
         public bool NetAspectAttribute = true;

          public MyAspect(byte value)
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
