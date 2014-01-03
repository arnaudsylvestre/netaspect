using System;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Bugs
{
    [TestFixture]
    public class NotRenamedInAssemblyTest : AcceptanceTest
    {
        protected override Action Execute()
        {
            return () =>
                {
                    string res = new MyClassToWeave().CheckNotRenameInAssembly();
                    Assert.AreEqual("Weaved", res);
                };
        }
    }
}