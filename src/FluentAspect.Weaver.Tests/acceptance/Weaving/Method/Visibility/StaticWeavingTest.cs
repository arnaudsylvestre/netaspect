using System;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Visibility
{
    [TestFixture]
    public class StaticWeavingTest : AcceptanceTest
    {
        protected override Action Execute()
        {
            return () =>
                {

                    throw new NotImplementedException(); string res = MyClassToWeave.CheckStatic(new BeforeParameter { Value = "not before" });
                    Assert.AreEqual("Value set in before", res);
                };
        }
    }
}