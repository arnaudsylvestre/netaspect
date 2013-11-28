using System;
using System.Reflection;
using FluentAspect.Core.Core;

namespace FluentAspect.Sample
{
   class CheckWithGenericsInterceptor : IInterceptor
    {
      public void Before(object thisObject, MethodInfo methodInfo_P, object[] parameters)
      {
      }

      public void After(object thisObject, MethodInfo methodInfo_P, object[] parameters, ref object result_P)
      {
         result_P = result_P + "Weaved";
      }

      public void OnException(object thisObject, MethodInfo methodInfo_P, object[] parameters, Exception e)
      {
      }
    }
}
