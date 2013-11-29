using System.Reflection;
using FluentAspect.Weaver.Core;
using Moq;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.Core
{
    [TestFixture]
    public class WeaverCoreTest
    {
        [Test]
        public void CheckLaunchWeavers()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var mock = new Mock<IWeaver>();
            mock.Setup(w => w.Weave(assembly));
            var weaver = new WeaverCore();
            weaver.Weave(assembly);
            mock.Verify(w => w.Weave(assembly));
        }
         
    }

    public interface IWeaver
    {
        void Weave(Assembly assembly);
    }
}