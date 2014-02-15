using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.ParameterName
{
    public class AfterMethodParameterNameParameterWithRealTypeEnumReferencedInInterceptorTest : NetAspectTest<AfterMethodParameterNameParameterWithRealTypeEnumReferencedInInterceptorTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.AreEqual(TestEnum.A, MyAspect.I);
               var classToWeave_L = new ClassToWeave();
                var testEnum = TestEnum.B;
                classToWeave_L.Weaved(testEnum);
               Assert.AreEqual(TestEnum.B, MyAspect.I);
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
         public void Weaved(TestEnum i)
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
         }
      }
   }

   
}