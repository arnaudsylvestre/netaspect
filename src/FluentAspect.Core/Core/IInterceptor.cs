using System;
using System.Reflection;

namespace FluentAspect.Core.Core
{
   public interface IInterceptor
   {
      void Before(object thisObject, MethodInfo methodInfo_P, object[] parameters);
      void After(object thisObject, MethodInfo methodInfo_P, object[] parameters, ref object result_P);
      void OnException(object thisObject, MethodInfo methodInfo_P, object[] parameters, Exception e);
   }
}