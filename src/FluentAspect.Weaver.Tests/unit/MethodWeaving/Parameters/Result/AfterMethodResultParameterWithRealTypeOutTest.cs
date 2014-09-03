using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.Result
{
   public class AfterMethodResultParameterWithRealTypeOutTest :
      NetAspectTest<AfterMethodResultParameterWithRealTypeOutTest.ClassToWeave>
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
                        string.Format(
                           "impossible to out the parameter 'result' in the method After of the type '{0}'",
                           typeof (MyAspect).FullName)
                  });
      }

      public class ClassToWeave
      {
         [MyAspect]
         public string Weaved()
         {
            return "NeverUsedValue";
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public void After(out string result)
         {
            result = "MyNewValue";
         }
      }
   }
}
