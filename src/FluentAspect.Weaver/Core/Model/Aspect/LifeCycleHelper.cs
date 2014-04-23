using System;

namespace NetAspect.Weaver.Core.Model.Aspect
{
   public static class LifeCycleHelper
   {
      public static LifeCycle Convert(string lifeCycleValue)
      {
         return (LifeCycle) Enum.Parse(typeof (LifeCycle), lifeCycleValue, true);
      }
   }
}