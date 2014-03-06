using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Parameters
{
   public class AfterMethodParametersParameterWithRealTypeTest : NetAspectTest<AfterMethodParametersParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.IsNull(MyAspect.Parameters);
               var classToWeave_L = new ClassToWeave();
               classToWeave_L.Weaved(12);
               Assert.AreEqual(new object[] {12}, MyAspect.Parameters);
            };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public void Weaved(int i)
         {

         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static object[] Parameters;

         public void After(object[] parameters)
         {
            Parameters = parameters;
         }
      }
   }

   
}