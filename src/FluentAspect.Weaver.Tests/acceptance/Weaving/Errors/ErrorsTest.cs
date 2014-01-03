using System;
using System.Collections.Generic;
using FluentAspect.Weaver.Core.Errors;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Errors
{
    public class ErrorsTest : AcceptanceTest
    {
        protected override void EnsureErrorHandler(ErrorHandler errorHandler)
        {
            Assert.AreEqual(new List<string>
                {
                    "A method declared in interface can not be weaved : InterfaceToWeaveWithAttributes.CheckBeforeWithAttributes",
                    "An abstract method can not be weaved : AbstractClassToWeaveWithAttributes.CheckBeforeWithAttributes",
                }, errorHandler.Warnings);
            Assert.AreEqual(new List<string>
                {
                }, errorHandler.Errors);
        }

        protected override Action Execute()
        {
            return () => { };
        }
    }
}