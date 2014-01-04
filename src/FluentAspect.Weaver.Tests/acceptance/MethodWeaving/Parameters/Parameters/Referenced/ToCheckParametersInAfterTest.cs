using System;
using FluentAspect.Sample.MethodWeaving.Parameters;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.MethodWeaving.Parameters.Parameters
{
    [TestFixture]
    public class ToCheckParametersReferencedInAfterTest : AcceptanceTest
    {
        protected override Action Execute()
        {
            return () =>
                {
                    new ToCheckParametersReferencedInAfter().Check("parameter", 3);
                    
                };
        }
    }
}