using System;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests
{
   [TestFixture]
   public class ParametersTest : AcceptanceTest
   {

      protected override Action Execute()
      {
         return () =>
            {
               string res = new MyClassToWeave().CheckWithParameters("Weaved with parameters");
               Assert.AreEqual("Weaved with parameters", res);
            };
      }
   }
}
