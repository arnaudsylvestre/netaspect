using System;
using System.Collections.Generic;
using NUnit.Framework;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.Result
{
   public class BeforeCallMethodResultParameterWithRealTypeTest :
      NetAspectTest<BeforeCallMethodResultParameterWithRealTypeTest.ClassToWeave>
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
                              "The parameter 'result' in the interceptor BeforeCallMethod of the aspect {0} is unknown. Expected one of : called, calledparameters, method, caller, callerparameters, callermethod, columnnumber, linenumber, filepath, filename", typeof(MyAspect).FullName)
                    });
       }
      public class ClassToWeave
      {
         [MyAspect]
         public string Method()
         {
            return "Hello";
         }

         public string Weaved()
         {
            return Method();
         }
      }

      public class MyAspect : Attribute
      {
         public static string Result;
         public bool NetAspectAttribute = true;

         public void BeforeCallMethod(string result)
         {
            Result = result;
         }
      }
   }
}
