using System;
using System.Reflection;
using FluentAspect.Core.Core;

namespace FluentAspect.Sample
{
   internal class CheckBeforeInterceptor : IInterceptor
   {
      public void Before(object thisObject, MethodInfo methodInfo_P, object[] parameters)
      {
         ((BeforeParameter) parameters[0]).Value = "Value set in before";
      }

      public void After(object thisObject, MethodInfo methodInfo_P, object[] parameters, ref object result_P)
      {
      }

      public void OnException(object thisObject, MethodInfo methodInfo_P, object[] parameters, Exception e)
      {
      }
   }
}
