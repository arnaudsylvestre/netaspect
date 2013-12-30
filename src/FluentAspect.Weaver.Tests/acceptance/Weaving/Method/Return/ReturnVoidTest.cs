using System;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests
{
   [TestFixture]
   public class ReturnVoidTest : AcceptanceTest
   {

      protected override Action Execute()
      {
         return () =>
            {
               new MyClassToWeave().CheckWithVoid();
            };
      }
   }
}
