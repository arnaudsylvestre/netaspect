using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.ParameterName
{
   public class AfterMethodParameterNameParameterWithRealTypeStructReferencedInInterceptorTest :
      NetAspectTest<AfterMethodParameterNameParameterWithRealTypeStructReferencedInInterceptorTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(0, MyAspect.I.A);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved(new TestStruct {A = 12});
            Assert.AreEqual(12, MyAspect.I.A);
         };
      }

      public struct TestStruct
      {
         public int A;
      }

      public class ClassToWeave
      {
         [MyAspect]
         public void Weaved(TestStruct i)
         {
         }
      }

      public class MyAspect : Attribute
      {
         public static TestStruct I;
         public bool NetAspectAttribute = true;

         public void AfterMethod(ref TestStruct i)
         {
            I = i;
         }
      }
   }
}
