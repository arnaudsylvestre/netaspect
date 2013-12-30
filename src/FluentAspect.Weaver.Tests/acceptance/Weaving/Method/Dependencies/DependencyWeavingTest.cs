using System;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests
{
   public class DependencyWeavingTest : AcceptanceTest
   {
      protected override Action Execute()
      {
         return () =>
            {
               try
               {
                  new MyClassToWeave().CheckDependency(null);
                  Assert.Fail();
               }
               catch (ArgumentNullException)
               {
               }
               
            };
      }
   }
}