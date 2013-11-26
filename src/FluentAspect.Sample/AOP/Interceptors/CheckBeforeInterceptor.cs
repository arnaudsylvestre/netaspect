using System;
using System.IO;
using FluentAspect.Core.Core;
using FluentAspect.Core.Expressions;

namespace FluentAspect.Sample
{
   class CheckBeforeInterceptor : IInterceptor
    {
        public void Before(MethodCall call_P)
        {
            ((BeforeParameter)call_P.Parameters[0]).Value = "Value set in before";
        }

       public void After(MethodCall call_P, MethodCallResult result_P)
       {
       }

      public void OnException(MethodCall callP_P, ExceptionResult e)
      {

      }
    }
}
