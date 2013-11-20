using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using SheepAspect.Compile;

namespace FluentAspect.Weaver.Tests
{
    [TestFixture]
    public class WeaverTest
    {
       [Test]
       public void CheckSimpleWeave()
       {
          const string asm = "FluentAspect.Sample.exe";
          WeaverTool weaver_L = new WeaverTool();
          var compilerSettings_L = new CompilerSettings();
          compilerSettings_L.AspectAssemblyFiles.Add(asm);
          compilerSettings_L.TargetAssemblyFiles.Add(asm);
          compilerSettings_L.BaseDirectory = Environment.CurrentDirectory;
          weaver_L.Weave(compilerSettings_L);
          var process_L = Process.Start("FluentAspect.Sample.exe");
          process_L.WaitForExit();
          Assert.AreEqual(0, process_L.ExitCode);
       }

       [Test]
       public void CheckWithoutSimpleWeave()
       {
          const string asm = "FluentAspect.Sample.exe";
          WeaverTool weaver_L = new WeaverTool();
          var compilerSettings_L = new CompilerSettings();
          compilerSettings_L.AspectAssemblyFiles.Add(asm);
          compilerSettings_L.TargetAssemblyFiles.Add(asm);
          compilerSettings_L.BaseDirectory = Environment.CurrentDirectory;
          //weaver_L.Weave(compilerSettings_L);
          var process_L = Process.Start("FluentAspect.Sample.exe");
          process_L.WaitForExit();
       }

       [Test]
       public void CheckSimpleWeaveCF()
       {
          try
          {
             const string asm = "FluentAspect.Sample.CF.exe";
             WeaverTool weaver_L = new WeaverTool();
             var compilerSettings_L = new CompilerSettings();
             compilerSettings_L.AspectAssemblyFiles.Add(asm);
             compilerSettings_L.TargetAssemblyFiles.Add(asm);
             compilerSettings_L.BaseDirectory = Environment.CurrentDirectory;
             weaver_L.Weave(compilerSettings_L);
          }
          catch (ReflectionTypeLoadException e)
          {
          }
       }
    }
}
