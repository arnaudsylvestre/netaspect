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
           foreach (var methodMatch in configuration.Properties)
           {
              foreach (var assemblyDefinition in methodMatch.AssembliesToScan)
              {
                 List<PropertyDefinition> methods = assemblyDefinition.GetAllProperties();
                 foreach (var methodDefinition_L in methods)
                 {
                    if (methodMatch.Matcher(methodDefinition_L) && methodMatch.Aspect != null)
                    {
                       if (!configurations.ContainsKey(methodDefinition_L.GetMethod))
                           configurations.Add(methodDefinition_L.GetMethod, new List<MethodWeavingConfiguration>());
                       configurations[methodDefinition_L.GetMethod].Add(new MethodWeavingConfiguration()
                       {
                           Type = methodMatch.Aspect.Type,
                           After = methodMatch.Aspect.AfterPropertyGet,
                           Before = methodMatch.Aspect.BeforePropertyGet,
                           OnException = methodMatch.Aspect.OnException,
                       });
                    }
                 }
              }
           }

           return configurations.Select(configuration_L => new AroundMethodWeaver(configuration_L.Value, configuration_L.Key)).Cast<IWeaveable>();
        }
    }
}