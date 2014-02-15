using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.ParameterName
{
    public class AfterMethodParameterNameParameterWithRealTypeDoubleReferencedTest : NetAspectTest<AfterMethodParameterNameParameterWithRealTypeDoubleReferencedTest.ClassToWeave>
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

         public void After(Double i)
         {
            I = i;
         }
      }
   }

   
}