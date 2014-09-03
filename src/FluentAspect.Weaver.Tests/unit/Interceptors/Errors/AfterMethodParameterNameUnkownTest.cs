using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Errors
{
   public class AfterMethodParameterNameUnkownTest : NetAspectTest<AfterMethodParameterNameUnkownTest.ClassToWeave>
   {
      protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
      {
         return errorHandler => errorHandler.Add(
            new ErrorReport.Error
            {
               Level = ErrorLevel.Error,
               Message = string.Format("The parameter 'unknown' is unknown. Expected one of : instance, method, parameters, i, linenumber, filename, filepath, result")
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

         public void After(string unknown)
         {
         }
      }
   }
}
