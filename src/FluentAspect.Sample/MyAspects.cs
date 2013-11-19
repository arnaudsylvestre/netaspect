using System;
using FluentAspect.Core;

namespace FluentAspect.Sample
{
   public class RaiseException : Exception
   {
      
   }

   public class MyAspects : FluentAspectDefinition
    {
        public void RaiseException(object callMethod)
        {
           throw new RaiseException();
        }

      public override void Setup()
      {
         WeaveMethodWhichMatches(m => m.Name == "MustRaiseExceptionAfterWeave", "RaiseException");
      }
    }
}
