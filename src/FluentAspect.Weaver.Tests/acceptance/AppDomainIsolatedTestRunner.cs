using System;

namespace FluentAspect.Weaver.Tests.acceptance
{
   public class AppDomainIsolatedTestRunner : MarshalByRefObject
   {
      public void Run(Action action)
      {
         action();
      }
   }
}