﻿using System;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.AssemblyAttributes
{
    [TestFixture]
    public class AssemblyAttributeTest : AcceptanceTest
    {
        protected override Action Execute()
        {
            return () =>
            {
                throw new NotImplementedException();
                    try
                    {
                        new MyClassToWeave().WeavedThroughAssembly();
                        Assert.Fail();
                    }
                    catch (NotSupportedException e)
                    {
                        Assert.AreEqual("Weaved through assembly", e.Message);
                    }
                };
        }
    }
}