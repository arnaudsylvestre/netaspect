using System;
using FluentAspect.Core.Core;
using FluentAspect.Core.Expressions;

namespace FluentAspect.Sample
{
   class CheckWithReturnInterceptor : IInterceptor
    {
        public void Before(MethodCall call_P)
        {
        }

       public void After(MethodCall call_P, MethodCallResult result_P)
       {
          result_P.Result = "Weaved";
       }

      public void OnException(MethodCall callP_P, Exception e)
      {

      }
    }
}
