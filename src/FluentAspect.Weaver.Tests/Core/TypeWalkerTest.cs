﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Moq;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.Core
{
    [TestFixture]
    public class TypeWalkerTest
    {
        [Test]
        public void CheckWalk()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var types = new List<Type>();

            //new TypeWalker(t => true).Walk(assembly, types.Add);

            Assert.AreEqual(assembly.GetTypes(), types);
        }

        [Test]
        public void CheckWalkFiltered()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var types = new List<Type>();

            //new TypeWalker(t => t == typeof(MyTypeToWalk)).Walk(assembly, types.Add);

            Assert.AreEqual(new List<Type> { typeof(MyTypeToWalk) }, types);
        }
    }

    public class MyTypeToWalk
    {
        public string Method()
        {
            return "";
        }
    }
}