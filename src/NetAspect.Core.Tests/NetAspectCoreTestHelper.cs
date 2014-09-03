using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Core.Helpers;

namespace NetAspect.Core.Tests
{
   public class NetAspectCoreTestHelper
   {
      public static void UpdateMethod(Type type, string methodName, NetAspectWeavingMethod weavingModel, Action<object, MethodInfo> callWeavedMethod, AssertInstructions assert)
      {
         AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly("NetAspect.Core.Tests.dll");

         MethodDefinition methodDefinition = assembly.MainModule.GetType(type.FullName).Methods.First(m => m.Name == methodName);
         methodDefinition.Weave(weavingModel, new VariableDefinition(methodDefinition.ReturnType));
         string newAssemblyName = "NetAspect.Core.Tests.Weaved.dll";
         assembly.Write(newAssemblyName);
         ProcessHelper.Launch("peverify.exe", "\"" + Path.GetFullPath(newAssemblyName) + "\"");
         Type weavedType = Assembly.LoadFrom(newAssemblyName).GetType(type.FullName);
         object instance = Activator.CreateInstance(weavedType);
         MethodInfo methodInfo = weavedType.GetMethod(methodName);
         callWeavedMethod(instance, methodInfo);

         assembly = AssemblyDefinition.ReadAssembly(newAssemblyName);
         assert.Check(assembly.MainModule.GetType(type.FullName).Methods.First(m => m.Name == methodName));
      }
   }
}
