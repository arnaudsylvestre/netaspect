using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After
{
   public class AfterConstructorParametersParameterWithRealTypeTest : NetAspectTest<AfterConstructorParametersParameterWithRealTypeTest.ClassToWeave>
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
         public bool NetAspectAttribute = true;

         public static object[] Parameters;

         public void After(object[] parameters)
         {
            Parameters = parameters;
         }
      }
   }

   
}