using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Result
{
   public class AfterMethodResultParameterWithRealTypeOutTest : NetAspectTest<AfterMethodResultParameterWithRealTypeOutTest.ClassToWeave>
   {

       protected override Action<FluentAspect.Weaver.Core.Errors.ErrorHandler> CreateErrorHandlerProvider()
       {
           return errorHandler => errorHandler.Errors.Add(string.Format("impossible to out the parameter 'result' in the method After of the type '{0}'", typeof(MyAspect).FullName));
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