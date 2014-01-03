﻿using System;
using FluentAspect.Sample.docs;
using FluentAspect.Weaver.Tests.acceptance;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.docs
{
    [TestFixture]
    public class MyIntTest : AcceptanceTest
    {
        protected override Action Execute()
        {
            return () =>
                {
                    var myInt = new MyInt(4);
                    myInt.DivideBy(2);
                    Assert.AreEqual("Start Division 4 / 2", LogAttribute.Trace.ToString());
                };
        }
    }
}