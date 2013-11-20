using System;
using FluentAspect.Core;

namespace FluentAspect.Sample
{
   public class RaiseException : Exception
   {
      
   }

   public class MyAspectDefinition : FluentAspectDefinition
    {
        public static void RaiseException(Action callMethod)
        {
           throw new RaiseException();
        }

      public override void Setup()
      {
         WeaveMethodWhichMatches(m => m.Name == "MustRaiseExceptionAfterWeave", "RaiseException");
      }
    }
}
