using System;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Calls
{
    [TestFixture]
    public class AfterCallConstructorTest : AcceptanceTest
    {
        protected override Action Execute()
        {
            return () =>
                {
                    try
                    {
                        MyClassToWeaveFactory.Create();
                        Assert.Fail();
                    }
                    catch (NotSupportedException)
                    {
                    }
                };
        }
    }
}