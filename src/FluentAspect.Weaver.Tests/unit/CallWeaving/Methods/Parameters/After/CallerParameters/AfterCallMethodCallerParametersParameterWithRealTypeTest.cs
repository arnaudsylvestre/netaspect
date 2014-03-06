using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Methods.Parameters.After.CallerParameters
{
    public class AfterCallMethodCallerParametersParameterWithRealTypeTest : NetAspectTest<AfterCallMethodCallerParametersParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.IsNull(MyAspect.CallerParameters);
               var classToWeave_L = new ClassToWeave();
               classToWeave_L.Weaved(1, 2);
               Assert.AreEqual(new object[]
                   {
                       1,2
                   }, MyAspect.CallerParameters);
            };
      }

      public class ClassToWeave
      {

          [MyAspect]
          public string Method() {return "Hello";}

         public string Weaved(int param1, int param2)
         {
             return Method();
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static object[] CallerParameters;

         public void AfterCallMethod(object[] callerParameters)
         {
             CallerParameters = callerParameters;
         }
      }
   }

   
}