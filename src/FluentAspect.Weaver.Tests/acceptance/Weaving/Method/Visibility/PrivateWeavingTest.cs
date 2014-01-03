using System;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Visibility
{
    [TestFixture]
    public class PrivateWeavingTest : AcceptanceTest
    {
        protected override Action Execute()
        {
            return () =>
                {
                    string res =
                        new MyClassToWeaveWithAttributes(false).CallCheckBeforeWithAttributesPrivate(new BeforeParameter
                            {
                                Value = "not before"
                            });
                    Assert.AreEqual("Value set in before", res);
                };
        }
    }
}