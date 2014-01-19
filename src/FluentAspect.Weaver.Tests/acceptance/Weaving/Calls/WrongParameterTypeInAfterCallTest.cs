using System;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Calls
{
    [TestFixture]
    public class WrongParameterTypeInAfterCallTest : AcceptanceTest
    {
        protected override Action Execute()
        {
            return () =>
            {
                throw new NotImplementedException();
                        new MyClassToWeave().AfterCallParametersWithWrongType("", "");
                };
        }
    }
}