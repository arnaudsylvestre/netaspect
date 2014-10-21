using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.ParameterName
{
   public class AfterMethodParameterNameParameterWithRealTypeSingleTest :
      NetAspectTest<AfterMethodParameterNameParameterWithRealTypeSingleTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(0, MyAspect.I);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved(3F);
            Assert.AreEqual(3F, MyAspect.I);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public void Weaved(Single i)
         {
         }
      }

      public class MyAspect : Attribute
      {
         public static Single I;
         public bool NetAspectAttribute = true;

         public void AfterMethod(Single i)
         {
            I = i;
         }
      }
   }
}
