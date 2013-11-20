using System.Diagnostics;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests
{
    [TestFixture]
    public class WeaverToolTest
    {
        [Test]
        public void CheckSimpleWeave()
        {
            const string asm = "FluentAspect.Sample.exe";
            var weaver_L = new WeaverTool(asm);
            weaver_L.Weave();
            Process process_L = Process.Start("FluentAspect.Sample.exe");
            process_L.WaitForExit();
            Assert.AreEqual(0, process_L.ExitCode);
        }

        [Test]
        public void CheckSimpleWeaveCF()
        {
            const string asm = "FluentAspect.Sample.CF.exe";
            var weaver_L = new WeaverTool(asm);
            weaver_L.Weave();
        }

        [Test]
        public void CheckWithoutSimpleWeave()
        {
            const string asm = "FluentAspect.Sample.exe";
            //weaver_L.Weave(compilerSettings_L);
            Process process_L = Process.Start("FluentAspect.Sample.exe");
            process_L.WaitForExit();
        }
    }
}