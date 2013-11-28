using System;
using System.Reflection;
using FluentAspect.Sample;

namespace FluentAspect.Weaver.Tests
{
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