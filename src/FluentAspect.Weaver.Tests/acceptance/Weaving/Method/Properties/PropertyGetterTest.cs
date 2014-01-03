﻿using System;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Properties
{
    [TestFixture]
    public class PropertyGetterTest : AcceptanceTest
    {
        protected override Action Execute()
        {
            return () => { Assert.AreEqual("3", new MyClassToWeaveWithAttributes(false).PropertyGetter); };
        }
    }
}