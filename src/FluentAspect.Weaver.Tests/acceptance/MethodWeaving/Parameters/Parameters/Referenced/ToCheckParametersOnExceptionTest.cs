using System;
using FluentAspect.Sample.MethodWeaving.Problems.Errors;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.MethodWeaving.Parameters.Parameters.Referenced
{
    [TestFixture]
    public class ToCheckParametersReferencedOnExceptionTest : AcceptanceTest
    {
        protected override Action Execute()
        {
            return () =>
                {
                    try
                    {
                        new ToCheckParametersReferencedOnException().Check("parameter", 3);
                        Assert.Fail();
                    }
                    catch (Exception e)
                    {
                        Assert.AreEqual("Une exception de type 'System.Exception' a été levée.", e.Message);
                    }
                    
                };
        }
    }
}