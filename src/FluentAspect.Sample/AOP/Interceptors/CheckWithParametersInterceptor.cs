using System;
using FluentAspect.Core.Core;
using FluentAspect.Core.Expressions;

namespace FluentAspect.Sample
{
   public class CheckWithParametersInterceptor : IInterceptor
   {
      public void Before(MethodCall call_P)
      {
      }

      public void After(MethodCall call_P, MethodCallResult result_P)
      {
         result_P.Result = call_P.Parameters[0];
      }

      public void OnException(MethodCall callP_P, Exception e)
      {
      }
   }
}