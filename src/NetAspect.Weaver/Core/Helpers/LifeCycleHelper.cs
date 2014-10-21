using System;
using NetAspect.Weaver.Core.Model.Aspect;

namespace NetAspect.Weaver.Core.Helpers
{
   public static class LifeCycleHelper
   {
      public static LifeCycle Convert(string lifeCycleValue)
      {
         return (LifeCycle) Enum.Parse(typeof (LifeCycle), lifeCycleValue, true);
      }
   }
}
