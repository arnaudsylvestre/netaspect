using System;
using FluentAspect.Core.Core;
using FluentAspect.Core.Expressions;

namespace FluentAspect.Sample
{
   class CheckBeforeInterceptor : IInterceptor
    {
        public void Before(MethodCall call_P)
        {
            call_P.Parameters[0] = "Value set in before";
        }

       public void After(MethodCall call_P, MethodCallResult result_P)
       {
       }

      public void OnException(MethodCall callP_P, ExceptionResult e)
      {

      }
    }
}
