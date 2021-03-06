using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.ParameterName
{
   public class AfterMethodParameterNameParameterWithGenericMethodTypeTest :
      NetAspectTest<AfterMethodParameterNameParameterWithGenericMethodTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(null, MyAspect.I);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved(12);
            Assert.AreEqual(12, MyAspect.I);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public void Weaved<T>(T i)
         {
         }
      }

      public class MyAspect : Attribute
      {
         public static object I;
         public bool NetAspectAttribute = true;

         public void AfterMethod(object i)
         {
            I = i;
         }
      }
   }
}
