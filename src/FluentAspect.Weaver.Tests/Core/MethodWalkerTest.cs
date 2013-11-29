using System;
using System.Collections.Generic;
using System.Reflection;
using Moq;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.Core
{
    [TestFixture]
    public class MethodWalkerTest
    {
        [Test]
        public void CheckWalk()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var mock = new Mock<Action<MethodInfo>>();
            mock.Setup(h => h(It.IsAny<MethodInfo>()));

            new MethodWalker(a => new[] { typeof(MyTypeToWalk) }).Walk(assembly, mock.Object);

            mock.Verify(h => h(It.IsAny<MethodInfo>()), Times.Exactly(assembly.GetTypes().Length));
            mock.Verify(h => h(typeof(MethodInfo).GetMethod("Method")), Times.Once());
        }
         
    }

    
}