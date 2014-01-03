using System;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Return
{
    [TestFixture]
    public class ReturnTest : AcceptanceTest
    {
        protected override Action Execute()
        {
            return () =>
                {
                    string res = new MyClassToWeave().CheckWithReturn();
                    Assert.AreEqual("Weaved", res);
                };
        }
    }
}