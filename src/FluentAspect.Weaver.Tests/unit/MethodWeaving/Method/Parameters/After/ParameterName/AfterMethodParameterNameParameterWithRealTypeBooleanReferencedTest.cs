using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.ParameterName
{
    public class AfterMethodParameterNameParameterWithRealTypeBooleanReferencedTest : NetAspectTest<AfterMethodParameterNameParameterWithRealTypeBooleanReferencedTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.AreEqual(false, MyAspect.I);
               var classToWeave_L = new ClassToWeave();
                var b = true;
                classToWeave_L.Weaved(ref b);
               Assert.AreEqual(true, MyAspect.I);
            };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public void Weaved(ref Boolean i)
         {

         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static Boolean I;

         public void After(Boolean i)
         {
            I = i;
         }
      }
   }

   
}