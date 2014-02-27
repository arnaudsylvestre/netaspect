using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.OnException
{
   public class OnExceptionConstructorParametersParameterWithRealTypeTest : NetAspectTest<OnExceptionConstructorParametersParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.IsNull(MyAspect.Parameters);
                try
                {
                    var classToWeave_L = new ClassToWeave(12);
                    Assert.Fail();
                }
                catch (Exception)
                {
                Assert.AreEqual(new object[] {12}, MyAspect.Parameters);
                    
                }
            };
      }

      public class ClassToWeave
      {
         [MyAspect]
          public ClassToWeave(int i)
         {
             throw new Exception();
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static object[] Parameters;

         public void OnException(object[] parameters)
         {
            Parameters = parameters;
         }
      }
   }

   
}