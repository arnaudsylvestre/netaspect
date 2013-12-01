using System;
using System.Reflection;
using FluentAspect.Core.Core;

namespace FluentAspect.Weaver.Tests
{
    class CheckThrowInterceptor : IInterceptor
    {
        public void Before(object thisObject, MethodInfo methodInfo_P, object[] parameters)
        { 
        }

        public void After(object thisObject, MethodInfo methodInfo_P, object[] parameters, ref object result_P)
        {
        }

        public void OnException(object thisObject, MethodInfo methodInfo_P, object[] parameters, Exception e)
        {
        }
    }

   public class SampleAspect
   {
      public string Sample<U>(U u)
      {
         var interceptor = new CheckThrowInterceptor();
         var args = new object[]
               {
                  u
               };
         MethodInfo method = GetType().GetMethod("Sample");
         string weavedResult;
         try
         {
            interceptor.Before(this, method, args);
            string result = SampleWeaved<U>(u);
            object handleResult = result;
            interceptor.After(this, method, args, ref handleResult);
            weavedResult = (string)handleResult;
         }
         catch (Exception e)
         {
            interceptor.OnException(this, method, args, e);
            throw;
         }
         return weavedResult;
      }

      public string SampleWeaved<T>(T t)
      {
         return "Not Weaved";
      } 
   }
}