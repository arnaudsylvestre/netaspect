using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.ParameterName
{
    public class AfterMethodParameterNameParameterWithRealTypeDoubleReferencedInBothTest : NetAspectTest<AfterMethodParameterNameParameterWithRealTypeDoubleReferencedInBothTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.AreEqual(0, MyAspect.I);
               var classToWeave_L = new ClassToWeave();
                var d = 2.3;
                classToWeave_L.Weaved(ref d);
                Assert.AreEqual(2.3, MyAspect.I);
                Assert.AreEqual(12, d);
            };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public void Weaved(ref Double i)
         {

         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static Double I;

         public void After(ref Double i)
         {
            I = i;
             i = 12;
         }
      }
   }

   
}