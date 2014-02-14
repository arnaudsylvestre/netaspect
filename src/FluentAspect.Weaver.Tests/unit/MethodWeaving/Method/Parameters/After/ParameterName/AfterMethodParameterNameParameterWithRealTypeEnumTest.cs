using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.ParameterName
{
   public class AfterMethodParameterNameParameterWithRealTypeEnumTest : NetAspectTest<AfterMethodParameterNameParameterWithRealTypeEnumTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.AreEqual(TestEnum.A, MyAspect.I);
               var classToWeave_L = new ClassToWeave();
               classToWeave_L.Weaved(TestEnum.B);
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

         public void After(TestEnum i)
         {
            I = i;
         }
      }
   }

   
}