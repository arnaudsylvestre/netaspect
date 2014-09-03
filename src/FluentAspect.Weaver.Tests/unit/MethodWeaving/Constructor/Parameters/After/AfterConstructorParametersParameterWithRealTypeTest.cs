using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.Parameters
{
   public class AfterConstructorParametersParameterWithRealTypeTest :
      NetAspectTest<AfterConstructorParametersParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.Parameters);
            var classToWeave_L = new ClassToWeave(12);
            Assert.AreEqual(new object[] {12}, MyAspect.Parameters);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public ClassToWeave(int i)
         {
         }
      }

      public class MyAspect : Attribute
      {
         public static object[] Parameters;
         public bool NetAspectAttribute = true;

         public void AfterConstructor(object[] parameters)
         {
            Parameters = parameters;
         }
      }
   }
}
