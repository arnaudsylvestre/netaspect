using FluentAspect.Weaver.Tests.Core;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit
{
    [TestFixture]
    public class UnitTest
    {
        [Test]
        public void Launch()
        {
            var assembly = AssemblyBuilder.Create();
            var method = assembly.WithType("MyClassToWeave").WithMethod("MyMethodToWeave");
            var aspect = assembly.WithMethodWeavingAspect("MyAspectAttribute");
            aspect.AddBefore();
            method.AddAspect(aspect);
            //method.WhichContainsThrowException();
            assembly.Save("TempAssembly.dll");


        }
    }
}