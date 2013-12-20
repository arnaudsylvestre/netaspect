using System;
using System.Reflection;

namespace FluentAspect.Weaver.Tests
{
    class CheckThrowInterceptor
    {

        public void Before(object instance, MethodInfo method, object[] parameters)
        {
        }

        public void After(object instance, MethodInfo method, object[] parameters, ref object result)
        {
        }

        public void OnException(object instance, MethodInfo method, object[] parameters, Exception exception)
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