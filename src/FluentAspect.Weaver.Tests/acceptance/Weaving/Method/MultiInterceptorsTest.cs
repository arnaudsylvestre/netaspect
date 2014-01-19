using System;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method
{
    [TestFixture]
    public class MultiInterceptorsTest : AcceptanceTest
    {
        protected override Action Execute()
        {
            throw new NotImplementedException();
            return () =>
                {
                    string res = new MyClassToWeave().CheckMulti(1);
                    Assert.AreEqual("3", res);
                };
        }
    }
}