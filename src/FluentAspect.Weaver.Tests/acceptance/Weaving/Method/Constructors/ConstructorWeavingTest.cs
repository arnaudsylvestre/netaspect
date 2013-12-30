using System;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests
{
   [TestFixture]
   public class ConstructorWeavingTest : AcceptanceTest
   {

      protected override Action Execute()
      {
         return () =>
            {
               new MyClassToWeaveWithAttributes(false);
               try
               {
                  new MyClassToWeaveWithAttributes(true);
                  Assert.Fail();
               }
               catch (NotSupportedException)
               {
               }
            };
      }
   }
}
