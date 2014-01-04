using System;
using FluentAspect.Sample.MethodWeaving.Parameters;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.MethodWeaving.Parameters.Parameters
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