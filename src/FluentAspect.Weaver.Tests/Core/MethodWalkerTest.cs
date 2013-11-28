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

    public class MethodWalker
    {
        private TypeWalker typeWalker;
        private Func<MethodInfo, bool> matcher;

        public MethodWalker(Func<MethodInfo, bool> matcher, TypeWalker typeWalker)
        {
            this.matcher = matcher;
            this.typeWalker = typeWalker;
        }

        public void Walk(IEnumerable<Type> types, Action<MethodInfo> methodWalkerHandler)
        {
            typeWalker.Walk(assembly, type =>
                {
                    
                });
            foreach (var type in typesProvider(assembly))
            {
                foreach (var methodInfo in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
                {
                    methodWalkerHandler(methodInfo);
                }
            }
                    
        }
    }
}