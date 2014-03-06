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
    public class AroundMethodParameterBuilderWeaver : IWeaverBuilder
    {
       public class ParameterWeavingConfiguration
       {
          public MethodWeavingConfiguration Configuration { get; set; }
          public ParameterDefinition Parameter { get; set; }
       }

        public IEnumerable<IWeaveable> BuildWeavers(WeavingConfiguration configuration)
        {
           var configurations = new Dictionary<MethodDefinition, List<ParameterWeavingConfiguration>>();
           foreach (var methodMatch in configuration.Parameters)
           {
              foreach (var assemblyDefinition in methodMatch.AssembliesToScan)
              {
                 List<MethodDefinition> methods = assemblyDefinition.GetAllMethods();
                 foreach (var methodDefinition_L in methods)
                 {
                     foreach (var parameterDefinition in methodDefinition_L.Parameters)
                     {
                         if (methodMatch.Matcher(parameterDefinition) && methodMatch.Aspect != null)
                         {
                             if (!configurations.ContainsKey(methodDefinition_L))
                                 configurations.Add(methodDefinition_L, new List<ParameterWeavingConfiguration>());
                             configurations[methodDefinition_L].Add(new ParameterWeavingConfiguration() 
                             {
                                Configuration = new MethodWeavingConfiguration()
                             {
                                 Type = methodMatch.Aspect.Type,
                                 After = methodMatch.Aspect.AfterParameter,
                                 Before = methodMatch.Aspect.BeforeParameter,
                                 OnException = methodMatch.Aspect.OnExceptionParameter,
                             }, Parameter = parameterDefinition});
                         }
                     }
                    
                 }
              }
           }

           return configurations.Select(configuration_L => new AroundMethodWeaver(configuration_L.Value.Select(a => a.Configuration).ToList(), configuration_L.Key, weave_P => new ParameterWeaver(weave_P, null))).Cast<IWeaveable>();
        }
    }
}