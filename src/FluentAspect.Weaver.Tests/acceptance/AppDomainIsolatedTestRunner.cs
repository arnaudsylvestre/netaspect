using System;

namespace FluentAspect.Weaver.Tests
{
   public class AppDomainIsolatedTestRunner : MarshalByRefObject
   {
      public void Run(Action action)
      {
         action();
      }
   }
}