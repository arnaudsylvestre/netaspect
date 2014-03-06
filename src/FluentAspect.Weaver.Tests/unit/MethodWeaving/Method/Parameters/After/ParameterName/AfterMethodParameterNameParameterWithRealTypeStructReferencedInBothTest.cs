using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.ParameterName
{
   public class AfterMethodParameterNameParameterWithRealTypeStructReferencedInBothTest : NetAspectTest<AfterMethodParameterNameParameterWithRealTypeStructReferencedInBothTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.AreEqual(0, MyAspect.I.A);
               var classToWeave_L = new ClassToWeave();
               var testStruct_L = new TestStruct {A = 12};
               classToWeave_L.Weaved(ref testStruct_L);
               Assert.AreEqual(12, MyAspect.I.A);
               Assert.AreEqual(55, testStruct_L.A);
            };
      }

      public struct TestStruct
      {
         public int A;
      }

      public class ClassToWeave
      {
         [MyAspect]
         public void Weaved(ref TestStruct i)
         {

         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static TestStruct I;

         public void After(ref TestStruct i)
         {
            I = i;
            i = new TestStruct()
               {
                  A = 55
               };
         }
      }
   }
}