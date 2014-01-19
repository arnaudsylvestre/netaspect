using System;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Calls
{
    [TestFixture]
    public class LineNumberCallTest : AcceptanceTest
    {
        protected override Action Execute()
        {
            return () =>
            {
                throw new NotImplementedException();
                    try
                    {
                        new MyClassToWeave().CallWeavedOnCallAfter();
                        Assert.Fail();
                    }
                    catch (Exception e)
                    {
                        Assert.AreEqual("101 : 13 : MyClassToWeave.cs", e.Message);
                    }
                };
        }
    }
}