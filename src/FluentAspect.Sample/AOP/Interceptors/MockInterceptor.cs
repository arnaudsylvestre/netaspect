using System;
using FluentAspect.Core.Core;
using FluentAspect.Core.Expressions;

namespace FluentAspect.Sample
{
   public class MockInterceptor : IInterceptor
   {


      public void Before(MethodCall call_P)
      {
         throw new NotImplementedException();
      }

      public void After(MethodCall call_P, MethodCallResult result_P)
      {
         throw new NotImplementedException();
      }

      public void OnException(MethodCall callP_P, Exception e)
      {
         throw new NotImplementedException();
      }
   }
}