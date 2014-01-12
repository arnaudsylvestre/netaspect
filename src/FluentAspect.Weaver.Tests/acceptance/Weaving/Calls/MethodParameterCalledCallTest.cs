﻿using System;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Calls
{
    [TestFixture]
   public class MethodParameterCalledCallTest : AcceptanceTest
    {
        protected override Action Execute()
        {
            return () =>
                {
                    try
                    {
                        new MyClassToWeave().CallWeavedOnCallAfterWithParameterCalled();
                        Assert.Fail();
                    }
                    catch (Exception e)
                    {
                       Assert.AreEqual("Parameter intercepted", e.Message);
                    }
                };
        }
    }
}