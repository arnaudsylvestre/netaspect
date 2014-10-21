using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.ParameterName
{
   public class AfterMethodParameterNameParameterWithRealTypeInt64ReferencedTest :
      NetAspectTest<AfterMethodParameterNameParameterWithRealTypeInt64ReferencedTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(0, MyAspect.I);
            var classToWeave_L = new ClassToWeave();
            Int64 i = 12;
            classToWeave_L.Weaved(ref i);
            Assert.AreEqual(12, MyAspect.I);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public void Weaved(ref Int64 i)
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
