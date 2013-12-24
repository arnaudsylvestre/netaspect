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

      public void ReadConfiguration(IEnumerable<Type> types, WeavingConfiguration configuration)
      {
         bool configurationFound = false;
         foreach (var weaverEngine_L in engines)
         {
            try
            {
                weaverEngine_L.ReadConfiguration(types, configuration);
               configurationFound = true;
            }
            catch (ConfigurationNotFoundException)
            {
            }
         }
          if (!configurationFound)
              throw new ConfigurationNotFoundException();
      }
   }
}