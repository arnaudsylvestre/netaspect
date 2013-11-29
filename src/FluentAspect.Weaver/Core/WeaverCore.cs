using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core
{
   public class WeaverCore
   {
      private IConfigurationReader configurationReader;
      private IWeaverBuilder weaverBuilder;

      public WeaverCore(IConfigurationReader configurationReader_P, IWeaverBuilder weaverBuilder_P)
      {
         configurationReader = configurationReader_P;
         weaverBuilder = weaverBuilder_P;
      }

      public void Weave(string assemblyFilePath)
      {
         var configuration_L = configurationReader.ReadConfiguration(Assembly.LoadFrom(assemblyFilePath).GetTypes());
         var assemblyDefinition = AssemblyDefinition.ReadAssembly(assemblyFilePath, new ReaderParameters(ReadingMode.Immediate) { ReadSymbols = true });
         var weavers = weaverBuilder.BuildWeavers(assemblyDefinition, configuration_L);
         foreach (var weaver_L in weavers)
         {
            weaver_L.Weave();
         }
         Clean(assemblyDefinition);
         assemblyDefinition.Write(assemblyFilePath, new WriterParameters()
         {
            WriteSymbols = true,
         });
      }

      private void Clean(AssemblyDefinition assemblyDefinition)
      {
         configurationReader.Clean(assemblyDefinition);
         CleanSelfReferences(assemblyDefinition);
      }

      private static void CleanSelfReferences(AssemblyDefinition assemblyDefinition)
      {
         foreach (var moduleDefinition in assemblyDefinition.Modules)
         {
            var same = (from r in moduleDefinition.AssemblyReferences where r.FullName == assemblyDefinition.FullName select r).ToList();
            foreach (var reference in same)
            {
               moduleDefinition.AssemblyReferences.Remove(reference);
            }
         }
      }
   }
}
