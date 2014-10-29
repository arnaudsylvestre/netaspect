using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.Aspects.Parameters.Members
{
   public class AspectParameterInParameterTest :
      NetAspectTest<AspectParameterInParameterTest.ClassToWeave>
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

          public void Weaved([MyAspect(3)] int i)
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

          public void AfterMethodForParameter(int parameterValue)
         {
            I = parameterValue;
              Max = _max;
         }
      }
   }
}
