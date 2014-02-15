using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.ParameterName
{
    public class AfterMethodParameterNameParameterWithRealTypeEnumReferencedInBothTest : NetAspectTest<AfterMethodParameterNameParameterWithRealTypeEnumReferencedInBothTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.AreEqual(TestEnum.A, MyAspect.I);
               var classToWeave_L = new ClassToWeave();
                var testEnum = TestEnum.B;
                classToWeave_L.Weaved(ref testEnum);
                Assert.AreEqual(TestEnum.B, MyAspect.I);
                Assert.AreEqual(TestEnum.C, testEnum);
            };
      }

      public enum TestEnum
      {
         A,
         B,
         C,
      }

      public class ClassToWeave
      {
         [MyAspect]
         public void Weaved(ref TestEnum i)
         {

         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static TestEnum I;

         public void After(ref TestEnum i)
         {
            I = i;
             i = TestEnum.C;
         }
      }
   }

   
}