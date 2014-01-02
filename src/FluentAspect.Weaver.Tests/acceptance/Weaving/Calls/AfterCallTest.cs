using System;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Calls
{
   [TestFixture]
   public class AfterCallTest : AcceptanceTest
   {

      protected override Action Execute()
      {
         return () =>
            {
               try
               {
                  new MyClassToWeave().CallWeavedOnCallAfter("Hello");
                  Assert.Fail();
               }
               catch (NotSupportedException)
               {
               }
            };
      }
   }
}
