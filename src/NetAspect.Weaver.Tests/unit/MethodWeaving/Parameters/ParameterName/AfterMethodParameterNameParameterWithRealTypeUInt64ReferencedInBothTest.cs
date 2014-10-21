using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.ParameterName
{
   public class AfterMethodParameterNameParameterWithRealTypeUInt64ReferencedInBothTest :
      NetAspectTest<AfterMethodParameterNameParameterWithRealTypeUInt64ReferencedInBothTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(0, MyAspect.I);
            var classToWeave_L = new ClassToWeave();
            UInt64 i_L = 12;
            classToWeave_L.Weaved(ref i_L);
            Assert.AreEqual(12, MyAspect.I);
            Assert.AreEqual(55, i_L);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public void Weaved(ref UInt64 i)
         {
         }
      }

      public class MyAspect : Attribute
      {
         public static UInt64 I;
         public bool NetAspectAttribute = true;

         public void AfterMethod(ref UInt64 i)
         {
            I = i;
            i = 55;
         }
      }
   }
}
