using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.Parameters
{
   public class AfterMethodParametersParameterWithRealTypeTest :
      NetAspectTest<AfterMethodParametersParameterWithRealTypeTest.ClassToWeave>
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
         public static object[] Parameters;
         public bool NetAspectAttribute = true;

         public void AfterMethod(object[] parameters)
         {
            Parameters = parameters;
         }
      }
   }
}
