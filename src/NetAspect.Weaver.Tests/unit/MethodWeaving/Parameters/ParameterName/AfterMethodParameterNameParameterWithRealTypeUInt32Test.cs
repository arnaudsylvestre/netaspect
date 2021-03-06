using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.ParameterName
{
   public class AfterMethodParameterNameParameterWithRealTypeUInt32Test :
      NetAspectTest<AfterMethodParameterNameParameterWithRealTypeUInt32Test.ClassToWeave>
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
         public void Weaved(UInt32 i)
         {
         }
      }

      public class MyAspect : Attribute
      {
         public static UInt32 I;
         public bool NetAspectAttribute = true;

         public void AfterMethod(UInt32 i)
         {
            I = i;
         }
      }
   }
}
