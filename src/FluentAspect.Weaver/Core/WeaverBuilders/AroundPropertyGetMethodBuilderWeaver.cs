using System.Collections.Generic;
using System.Linq;
using FluentAspect.Weaver.Core.Configuration;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.Model.Adapters;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Methods;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.WeaverBuilders
{
    public class AroundPropertyGetMethodBuilderWeaver : IWeaverBuilder
    {
        public IEnumerable<IWeaveable> BuildWeavers(WeavingConfiguration configuration)
        {
           var configurations = new Dictionary<MethodDefinition, List<MethodWeavingConfiguration>>();
           foreach (PropertyMatch methodMatch in configuration.Properties)
           {
              foreach (var assemblyDefinition in methodMatch.AssembliesToScan)
              {
                 List<PropertyDefinition> methods = assemblyDefinition.GetAllProperties();
                 foreach (var methodDefinition_L in methods)
                 {
                    if (methodMatch.Matcher(methodDefinition_L) && methodMatch.MethodWeavingInterceptors != null)
                    {
                       if (!configurations.ContainsKey(methodDefinition_L))
                          configurations.Add(methodDefinition_L, new List<MethodWeavingConfiguration>());
                       configurations[methodDefinition_L].Add(methodMatch.MethodWeavingInterceptors);
                    }
                 }
              }
           }

           return configurations.Select(configuration_L => new AroundMethodWeaver(configuration_L.Value, configuration_L.Key)).Cast<IWeaveable>();
        }
    }
}