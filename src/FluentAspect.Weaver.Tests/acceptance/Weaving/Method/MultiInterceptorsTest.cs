using System;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests
{
   [TestFixture]
   public class MultiInterceptorsTest : AcceptanceTest
   {

      protected override Action Execute()
      {
         return () =>
            {
               var res = new MyClassToWeave().CheckMulti(1);
               Assert.AreEqual("3", res);
            };
      }
   }
}
