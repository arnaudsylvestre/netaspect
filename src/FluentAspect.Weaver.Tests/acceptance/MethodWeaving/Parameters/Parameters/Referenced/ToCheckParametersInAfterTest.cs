using System;
using FluentAspect.Sample.MethodWeaving.Problems.Errors;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.MethodWeaving.Parameters.Parameters.Referenced
{
    [TestFixture]
    public class ToCheckParametersReferencedInAfterTest : AcceptanceTest
    {
        protected override Action Execute()
        {
            return () =>
                {
                    throw new NotImplementedException();
                    new ToCheckParametersReferencedInAfter().Check("parameter", 3);
                    
                };
        }
    }
}