using System;
using FluentAspect.Core.Expressions;

namespace FluentAspect.Core.Core
{
   public interface IInterceptor
   {
      void Before(MethodCall call_P);
      void After(MethodCall call_P, MethodCallResult result_P);
      void OnException(MethodCall callP_P, Exception e);
   }
}