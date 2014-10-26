using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.Interceptors.Errors
{
   public class AfterMethodParameterNameUnkownTest : NetAspectTest<AfterMethodParameterNameUnkownTest.ClassToWeave>
   {
      protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
      {
         return errorHandler => errorHandler.Add(
            new ErrorReport.Error
            {
               Level = ErrorLevel.Error,
               Message = string.Format("The parameter 'unknown' in the interceptor AfterMethod of the aspect {0} is unknown. Expected one of : instance, method, parameters, i, linenumber, columnnumber, filename, filepath, result", typeof(MyAspect).FullName)
            });
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

         public void AfterMethod(string unknown)
         {
         }
      }
   }
}
