using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.ParameterName
{
   public class AfterMethodParameterNameParameterWithRealTypeStructTest : NetAspectTest<AfterMethodParameterNameParameterWithRealTypeStructTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.AreEqual(0, MyAspect.I.A);
               var classToWeave_L = new ClassToWeave();
               classToWeave_L.Weaved(new TestStruct { A = 12});
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
         public bool NetAspectAttribute = true;

         public static TestStruct I;

         public void After(TestStruct i)
         {
            I = i;
         }
      }
   }

   
}