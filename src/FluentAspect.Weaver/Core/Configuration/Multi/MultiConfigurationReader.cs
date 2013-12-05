using System;
using System.Collections.Generic;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Configuration.Multi
{
   public class MultiConfigurationReader : IConfigurationReader
   {
      private IEnumerable<IConfigurationReader> engines;

      public MultiConfigurationReader(params IConfigurationReader[] engines_P)
      {
         engines = engines_P;
      }

      public WeavingConfiguration ReadConfiguration(IEnumerable<Type> types)
      {
         WeavingConfiguration configuration_L = new WeavingConfiguration();
         bool configurationFound = false;
         foreach (var weaverEngine_L in engines)
         {
            try
            {
               configuration_L.MergeWith(weaverEngine_L.ReadConfiguration(types));
               configurationFound = true;
            }
            catch (ConfigurationNotFoundException)
            {
            }
         }
         if (!configurationFound)
            throw new ConfigurationNotFoundException();
         return configuration_L;
      }

      public void Clean(AssemblyDefinition assemblyDefinition)
      {
         foreach (var configurationReader_L in engines)
         {
            configurationReader_L.Clean(assemblyDefinition);
         }
      }
   }
}