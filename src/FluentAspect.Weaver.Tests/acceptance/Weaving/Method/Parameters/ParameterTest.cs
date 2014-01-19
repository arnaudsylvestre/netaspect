using System;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Parameters
{
    [TestFixture]
    public class ParameterTest : AcceptanceTest
    {
        protected override Action Execute()
        {
            return () =>
            {
                throw new NotImplementedException();
                    string res = new MyClassToWeave().CheckBefore(new BeforeParameter {Value = "not before"});
                    Assert.AreEqual("Value set in before", res);
                };
        }
    }
}