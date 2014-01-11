using System;
using FluentAspect.Sample.MethodWeaving.Problems.Errors;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.MethodWeaving.Parameters.Parameters.Referenced
{
    [TestFixture]
    public class ToCheckParametersReferencedInBeforeTest : AcceptanceTest
    {
        protected override Action Execute()
        {
            return () =>
                {
                    new ToCheckParametersReferencedInBefore().Check("parameter", 3);
                };
        }
    }
}