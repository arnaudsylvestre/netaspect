using System;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests
{
   public class DependencyWithoutExceptionTest : AcceptanceTest
   {
      protected override Action Execute()
      {
         return () =>
            {
               new MyClassToWeave().CheckDependency("");
            };
      }
   }
}