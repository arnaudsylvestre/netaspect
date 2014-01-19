using System;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Calls
{
    [TestFixture]
    public class CallerTest : AcceptanceTest
    {
        protected override Action Execute()
        {
            return () =>
            {
                throw new NotImplementedException();
                    try
                    {
                        new MyClassToWeave().CallCheckCaller();
                        Assert.Fail();
                    }
                    catch (Exception e)
                    {
                        Assert.AreEqual("OK", e.Message);
                    }
                };
        }
    }
}