using System;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests
{
   [TestFixture]
   public class BeforeCallTest : AcceptanceTest
   {

      protected override Action Execute()
      {
         return () =>
            {
               try
               {
                  new MyClassToWeave().CallWeavedOnCall("Hello");
                  Assert.Fail();
               }
               catch (NotSupportedException)
               {
               }
            };
      }
   }
}
