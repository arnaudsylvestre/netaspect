#region

using System;
using FluentAspect.Sample;
using NUnit.Framework;

#endregion

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Visibility
{
    [TestFixture]
    public class ProtectedWeavingTest : AcceptanceTest
    {
        protected override Action Execute()
        {
            return () =>
                {
                    string res =
                        new MyClassToWeaveWithAttributes(false).CallCheckBeforeWithAttributesProtected(
                            new BeforeParameter
                                {
                                    Value = "not before"
                                });
                    Assert.AreEqual("Value set in before", res);
                };
        }
    }
}