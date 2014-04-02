using System;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Apis.AssemblyChecker.Peverify;
using Mono.Cecil;

namespace NetAspect.Core.Tests
{
    public class NetAspectCoreTestHelper
    {
        public static void UpdateMethod(Type type, string methodName, NetAspectWeavingMethod weavingModel, Action<object, MethodInfo> callWeavedMethod)
        {
            var assembly = AssemblyDefinition.ReadAssembly("NetAspect.Core.Tests.dll",
                                                           new ReaderParameters() { ReadSymbols = true });

            assembly.MainModule.GetType(type.FullName).Methods.First(m => m.Name == methodName).Weave(weavingModel);
            var newAssemblyName = "NetAspect.Core.Tests.Weaved.dll";
            assembly.Write(newAssemblyName, new WriterParameters() { WriteSymbols = true });
            ProcessHelper.Launch("peverify.exe", "\"" + Path.GetFullPath(newAssemblyName) + "\"");
            var weavedType = Assembly.LoadFrom(newAssemblyName).GetType(type.FullName);
            var instance = Activator.CreateInstance(weavedType);
            var methodInfo = weavedType.GetMethod(methodName);
            callWeavedMethod(instance, methodInfo);
        }
    }
}