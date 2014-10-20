using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Apis.AssemblyChecker;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Helpers;
using NetAspect.Weaver.Helpers.NetFramework;

namespace NetAspect.Weaver.Core.Assemblies
{
   public class AssemblyPool
   {
      private readonly Dictionary<Assembly, AssemblyDefinition> asms = new Dictionary<Assembly, AssemblyDefinition>();
      private readonly IAssemblyChecker assemblyChecker;
      private readonly Func<TypeDefinition, bool> typesToSave;

      public AssemblyPool(IAssemblyChecker assemblyChecker, Func<TypeDefinition, bool> typesToSave_P)
      {
         this.assemblyChecker = assemblyChecker;
         typesToSave = typesToSave_P;
      }

      public void Add(IEnumerable<Assembly> assemblies)
      {
         foreach (Assembly assembly_L in assemblies)
         {
            asms.Add(
               assembly_L,
               AssemblyDefinition.ReadAssembly(
                  assembly_L.GetAssemblyPath(),
                  new ReaderParameters(ReadingMode.Immediate)
                  {
                     ReadSymbols = true
                  }));
         }
      }

      public AssemblyDefinition GetAssemblyDefinition(Assembly assembly)
      {
         return asms[assembly];
      }

      public void Save(ErrorHandler errorHandler, Func<string, string> newAssemblyNameProvider)
      {
         foreach (var def in asms)
         {
            AssemblyDefinition assemblyDefinition_L = def.Value;
            Clean(assemblyDefinition_L);
            WeaveOneAssembly(def.Key.GetAssemblyPath(), assemblyDefinition_L, errorHandler, newAssemblyNameProvider);
         }
      }

      private void Clean(AssemblyDefinition assemblyDefinition_L)
      {
         if (typesToSave == null) return;
         foreach (TypeDefinition type in assemblyDefinition_L.MainModule.GetTypes().Where(typeDefinition_L => !typesToSave(typeDefinition_L)).ToList())
         {
            assemblyDefinition_L.MainModule.Types.Remove(type);
         }
      }


      private void WeaveOneAssembly(string getAssemblyPath,
         AssemblyDefinition assemblyDefinition,
         ErrorHandler errorHandler,
         Func<string, string> newAssemblyNameProvider)
      {
         string targetFileName = newAssemblyNameProvider(getAssemblyPath);
         assemblyDefinition.Write(
            targetFileName,
            new WriterParameters
            {
               WriteSymbols = true,
            });
         assemblyChecker.Check(targetFileName, errorHandler);
      }
   }
}
