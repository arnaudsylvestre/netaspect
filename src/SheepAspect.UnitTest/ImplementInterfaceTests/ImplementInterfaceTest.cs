﻿using System;
using Moq;
using NUnit.Framework;
using SheepAspect.Core;
using SheepAspect.MixinsAdvising;
using SheepAspect.Pointcuts.Impl;
using SheepAspect.UnitTest.TestHelper;
using FluentAssertions;

namespace SheepAspect.UnitTest.ImplementInterfaceTests
{
    public class ImplementInterfaceTest: AspectTestBase
    {
        private static Mock<ITestInterface> _interfaceMock;

        protected override Type TargetType()
        {
            return typeof(TestTarget);
        }

        protected override void SetupAspect(AspectDefinition aspect)
        {
            _interfaceMock = new Mock<ITestInterface>();
            aspect.Advise(
                new DeclareMixinFromMethodAdvice(CreatePointcuts<TypePointcut>(aspect, "TestPointcut", "Name:'TestTarget'"),
                    GetAspectMethod("MixinTestAdvice"), null, true));
        }

        public ITestInterface MixinTestAdvice()
        {
            return _interfaceMock.Object;
        }

        [Assert]
        public void CanMixinMethod()
        {
            var stubResult = new object();
            _interfaceMock.Setup(x => x.SomeMethod(123)).Returns(stubResult);
            var casted = (ITestInterface)Target;
            casted.SomeMethod(123).Should().Be(stubResult);
        }
    }

    public interface ITestInterface
    {
        object SomeMethod(int abc);
    }

    public class TestTarget
    {
    }
}