using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.Instance
{
   public class AfterMethodInstanceParameterWithRealTypeInStaticTest :
      NetAspectTest<AfterMethodInstanceParameterWithRealTypeInStaticTest.ClassToWeave>
   {
      protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
      {
         return
            errorHandler =>
               errorHandler.Add(
                  new ErrorReport.Error
                  {
                     Level = ErrorLevel.Error,
                     Message =
                        string.Format("the instance parameter can not be used for static method interceptors")
                  });
      }

      public class ClassToWeave
      {
         [MyAspect]
         public static void Weaved()
         {
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public void AfterMethod(ClassToWeave instance)
         {
         }
      }
   }
}
