using System;
using FluentAspect.Core;
using FluentAspect.Core.Expressions;

namespace FluentAspect.Sample
{
   public class RaiseException : Exception
   {
      
   }

    class MyInterceptor : IInterceptor
    {
        public void Before()
        {
        }

        public void After(Around.MethodCallResult ret)
        {
            ret.Result = "Weaved";
        }

        public void OnException(Exception e)
        {
        }
    }

   public class MyAspectDefinition : FluentAspectDefinition
    {
        public static void RaiseException(Action callMethod)
        {
           throw new RaiseException();
        }

      public override void Setup()
      {
          WeaveMethodWhichMatches<MyInterceptor>(m => m.Name == "MustRaiseExceptionAfterWeave");
      }
    }
}
