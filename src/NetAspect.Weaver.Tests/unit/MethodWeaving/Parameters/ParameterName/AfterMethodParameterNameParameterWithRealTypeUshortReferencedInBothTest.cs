using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.ParameterName
{
   public class AfterMethodParameterNameParameterWithRealTypeUshortReferencedInBothTest :
      NetAspectTest<AfterMethodParameterNameParameterWithRealTypeUshortReferencedInBothTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(0, MyAspect.I);
            var classToWeave_L = new ClassToWeave();
            ushort i_L = 12;
            classToWeave_L.Weaved(ref i_L);
            Assert.AreEqual(12, MyAspect.I);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public void Weaved(ref ushort i)
         {
         }
      }

      public class MyAspect : Attribute
      {
         public static ushort I;
         public bool NetAspectAttribute = true;

         public void AfterMethod(ref ushort i)
         {
            I = i;
            i = 55;
         }
      }
   }
}
