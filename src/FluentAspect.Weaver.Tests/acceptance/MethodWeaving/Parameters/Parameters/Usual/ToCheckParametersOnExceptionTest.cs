using System;
using FluentAspect.Sample.MethodWeaving.Parameters;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.MethodWeaving.Parameters.Parameters
{
    [TestFixture]
    public class ToCheckParametersOnExceptionTest : AcceptanceTest
    {
        protected override Action Execute()
        {
            return () =>
                {
                    try
                    {
                        new ToCheckParametersOnException().Check("parameter", 3);
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