using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.ParameterName
{
   public class AfterMethodParameterNameParameterWithRealTypeBooleanTest :
      NetAspectTest<AfterMethodParameterNameParameterWithRealTypeBooleanTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(false, MyAspect.I);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved(true);
            Assert.AreEqual(true, MyAspect.I);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public void Weaved(Boolean i)
         {
         }
      }

      public class MyAspect : Attribute
      {
         public static Boolean I;
         public bool NetAspectAttribute = true;

         public void AfterMethod(Boolean i)
         {
            I = i;
         }
      }
   }
}
