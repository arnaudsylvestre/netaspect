using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.Aspects.Parameters.Types
{
   public class AspectParameterInParameterWithUShortTypeTest :
      NetAspectTest<AspectParameterInParameterWithUShortTypeTest.ClassToWeave>
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
          private readonly ushort _value;
          public static int I;
          public static ushort Max;
         public bool NetAspectAttribute = true;

         public MyAspect(ushort value)
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
