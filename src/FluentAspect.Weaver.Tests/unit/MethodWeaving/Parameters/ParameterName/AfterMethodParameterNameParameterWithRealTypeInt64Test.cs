using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.ParameterName
{
   public class AfterMethodParameterNameParameterWithRealTypeInt64Test :
      NetAspectTest<AfterMethodParameterNameParameterWithRealTypeInt64Test.ClassToWeave>
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
         public void Weaved(Int64 i)
         {
         }
      }

      public class MyAspect : Attribute
      {
         public static Int64 I;
         public bool NetAspectAttribute = true;

         public void AfterMethod(Int64 i)
         {
            I = i;
         }
      }
   }
}
