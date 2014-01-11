using System;
using FluentAspect.Sample.MethodWeaving.Parameters.Parameters.Usual;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.MethodWeaving.Parameters.Parameters.Usual
{
    [TestFixture]
    public class ToCheckParametersInBeforeTest : AcceptanceTest
    {
        protected override Action Execute()
        {
            return () =>
                {
                    try
                    {
                        new ToCheckParametersInBefore().Check("parameter", 3);
                        Assert.Fail();
                    }
                    catch (ParametersException e)
                    {
                        Assert.AreEqual("parameter", e.Parameters[0]);
                        Assert.AreEqual(3, e.Parameters[1]);
                    }
                    
                };
        }
    }
}