using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.Aspects.Parameters.Types
{
   public class AspectParameterInParameterWithShortTypeTest :
      NetAspectTest<AspectParameterInParameterWithShortTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(0, MyAspect.I);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved(12);
            Assert.AreEqual(12, MyAspect.I);
            Assert.AreEqual(12, MyAspect.Max);
         };
      }

      public class ClassToWeave
      {

          public void Weaved([MyAspect(12)] int i)
         {
         }
      }

      public class MyAspect : Attribute
      {
          private readonly short _value;
          public static int I;
          public static short Max;
         public bool NetAspectAttribute = true;

         public MyAspect(short value)
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
