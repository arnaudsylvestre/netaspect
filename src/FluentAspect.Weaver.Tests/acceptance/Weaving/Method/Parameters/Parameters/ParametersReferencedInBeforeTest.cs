using System;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Parameters.Parameters
{
    [TestFixture]
    public class ParametersReferencedInBeforeTest : AcceptanceTest
    {
        protected override Action Execute()
        {
            return () =>
                {
                    string res = new MyClassToWeave().CheckWithParametersReferenced("Weaved with parameters");
                    Assert.AreEqual("NotWeaved", res);
                };
        }
    }
}