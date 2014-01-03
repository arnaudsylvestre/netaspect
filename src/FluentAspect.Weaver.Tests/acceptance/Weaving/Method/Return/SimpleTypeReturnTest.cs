using System;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Return
{
    [TestFixture]
    public class SimpleTypeReturnTest : AcceptanceTest
    {
        protected override Action Execute()
        {
            return () =>
                {
                    int res = new MyClassToWeave().CheckWithReturnSimpleType();
                    Assert.AreEqual(5, res);
                };
        }
    }
}