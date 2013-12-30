using System;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests
{
   [TestFixture]
   public class ParameterTest : AcceptanceTest
   {

      protected override Action Execute()
      {
         return () =>
            {
               string res = new MyClassToWeave().CheckBefore(new BeforeParameter {Value = "not before"});
               Assert.AreEqual("Value set in before", res);
            };
      }
   }
}
