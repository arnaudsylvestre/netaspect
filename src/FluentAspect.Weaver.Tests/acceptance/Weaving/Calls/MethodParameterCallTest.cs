using System;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Calls
{
    [TestFixture]
    public class MethodParameterCallTest : AcceptanceTest
    {
        protected override Action Execute()
        {
            return () =>
                {
                    try
                    {
                        new MyClassToWeave().CallWeavedOnCallAfterWithParameters("My parameter");
                        Assert.Fail();
                    }
                    catch (Exception e)
                    {
                        Assert.AreEqual("My parameter", e.Message);
                    }
                };
        }
    }
}