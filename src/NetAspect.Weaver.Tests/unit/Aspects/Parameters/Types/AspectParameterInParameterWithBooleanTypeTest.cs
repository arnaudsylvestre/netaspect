using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.Aspects.Parameters.Types
{
   public class AspectParameterInParameterWithBooleanTypeTest :
      NetAspectTest<AspectParameterInParameterWithBooleanTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(0, MyAspect.I);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved(12);
            Assert.AreEqual(12, MyAspect.I);
            Assert.True(MyAspect.Max);
         };
      }

      public class ClassToWeave
      {

          public void Weaved([MyAspect(true)] int i)
         {
         }
      }

      public class MyAspect : Attribute
      {
          private readonly bool _value;
          public static int I;
          public static bool Max;
         public bool NetAspectAttribute = true;

          public MyAspect(bool value)
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
