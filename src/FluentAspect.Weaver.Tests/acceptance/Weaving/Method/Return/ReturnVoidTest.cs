using System;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Return
{
    [TestFixture]
    public class ReturnVoidTest : AcceptanceTest
    {
        protected override Action Execute()
        {
            return () =>
            {
                throw new NotImplementedException(); new MyClassToWeave().CheckWithVoid();
            };
        }
    }
}