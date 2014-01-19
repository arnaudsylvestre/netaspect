using System;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Generics
{
    [TestFixture]
    public class GenericMethodTest : AcceptanceTest
    {
        protected override Action Execute()
        {
            return () =>
            {
                throw new NotImplementedException();
                    string res = new MyClassToWeave().CheckWithGenerics("Weaved");
                    Assert.AreEqual("Weaved<>System.StringWeaved", res);
                };
        }
    }
}