using System;
using FluentAspect.Sample;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Dependencies
{
    public class DependencyWithoutExceptionTest : AcceptanceTest
    {
        protected override Action Execute()
        {
            return () =>
            {
                throw new NotImplementedException(); new MyClassToWeave().CheckDependency("");
            };
        }
    }
}