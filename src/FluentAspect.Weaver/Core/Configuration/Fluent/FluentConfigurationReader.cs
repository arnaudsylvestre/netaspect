using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentAspect.Core;
using FluentAspect.Weaver.Weavers.Methods;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Fluent
{
   public class FluentConfigurationReader : IConfigurationReader
   {
      public WeavingConfiguration ReadConfiguration(IEnumerable<Type> types)
      {
         var fluentConfigurationType = GetFluentConfiguration(types);
         return ExtractWeavingConfiguration(fluentConfigurationType);
      }

      public void Clean(AssemblyDefinition assemblyDefinition)
      {
          foreach (var moduleDefinition in assemblyDefinition.Modules)
          {
              List<TypeDefinition> toDelete = new List<TypeDefinition>();
              foreach (var typeDefinition in moduleDefinition.GetTypes())
              {
                  if (typeDefinition.BaseType == null)
                      continue;
                    if (typeDefinition.BaseType.FullName == typeof (FluentAspectDefinition).FullName)
                    {
                        toDelete.Add(typeDefinition);
                    }
              }
              foreach (var typeDefinition in toDelete)
              {
                  moduleDefinition.Types.Remove(typeDefinition);
              }
          }
          
      }

      private WeavingConfiguration ExtractWeavingConfiguration(Type fluentConfigurationType_P)
      {
         WeavingConfiguration configuration_L = new WeavingConfiguration();
         var instance_L = (FluentAspectDefinition)Activator.CreateInstance(fluentConfigurationType_P);
         instance_L.Setup();
         var methodMatches_L = instance_L.methodMatches;
         foreach (var methodMatch_L in methodMatches_L)
         {
            configuration_L.Methods.Add(methodMatch_L);
         }
         return configuration_L;
      }

      private Type GetFluentConfiguration(IEnumerable<Type> types_P)
      {
         try
         {
            return (from t in types_P where t.BaseType == typeof(FluentAspectDefinition) select t).First();
         }
         catch (Exception)
         {
            throw new ConfigurationNotFoundException();
         }
      }
   }
}
