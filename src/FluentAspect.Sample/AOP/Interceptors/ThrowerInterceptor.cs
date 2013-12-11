using System;
using System.Reflection;
using FluentAspect.Core.Core;

namespace FluentAspect.Sample
{
   public class ThrowerInterceptor : IInterceptor
   {
      public void Before(object thisObject, MethodInfo methodInfo_P, object[] parameters)
      {
          if ((bool)parameters[0])
            throw new NotSupportedException();
      }

      public void After(object thisObject, MethodInfo methodInfo_P, object[] parameters, ref object result_P)
      {
      }

      public void OnException(object thisObject, MethodInfo methodInfo_P, object[] parameters, Exception e)
      {
      }
   }
}