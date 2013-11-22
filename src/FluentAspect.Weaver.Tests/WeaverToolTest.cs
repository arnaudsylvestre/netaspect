using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using NUnit.Framework;
using TypeAttributes = Mono.Cecil.TypeAttributes;

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

        }

        [Test]
        public void CheckCreateSimple()
        {
            var assemblyDefinition = AssemblyDefinition.CreateAssembly(new AssemblyNameDefinition("Test", new Version("1.0")), "Test", ModuleKind.Dll);
            var moduleDefinition = ModuleDefinition.CreateModule("TestModule", ModuleKind.Dll);
            assemblyDefinition.Modules.Add(moduleDefinition);
            moduleDefinition.Types.Add(new TypeDefinition("A", "ClassA", TypeAttributes.Class | TypeAttributes.Public));
            assemblyDefinition.Write("Test.dll");
        }

        [Test]
        public void CheckSimpleWeave2()
        {
            try
            {
                var assembly = Assembly.LoadFrom("FluentAspect.Sample.exe");
                var type = (from t in assembly.GetTypes() where t.Name == "MyClassToWeave" select t).First();
                Assert.AreEqual("Weaved", type.GetMethod("MustRaiseExceptionAfterWeave").Invoke(Activator.CreateInstance(type), new object[0]));

            }
            catch (ReflectionTypeLoadException e)
            {
                throw;
            }
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