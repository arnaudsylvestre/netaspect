using System.Collections.Generic;
using System.Linq;
using FluentAspect.Weaver.Core.Configuration;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Methods;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.WeaverBuilders
{
    public class AroundPropertySetMethodBuilderWeaver : IWeaverBuilder
    {
        public IEnumerable<IWeaveable> BuildWeavers(WeavingConfiguration configuration)
        {
            var configurations = new Dictionary<MethodDefinition, List<MethodWeavingConfiguration>>();
            foreach (var methodMatch in configuration.Properties)
            {
                foreach (AssemblyDefinition assemblyDefinition in methodMatch.AssembliesToScan)
                {
                    List<PropertyDefinition> methods = assemblyDefinition.GetAllProperties();
                    foreach (PropertyDefinition methodDefinition_L in methods)
                    {
                        if (methodMatch.Matcher(methodDefinition_L) && methodDefinition_L.SetMethod != null &&
                            methodMatch.Aspect != null)
                        {
                            if (!configurations.ContainsKey(methodDefinition_L.SetMethod))
                                configurations.Add(methodDefinition_L.SetMethod, new List<MethodWeavingConfiguration>());
                            configurations[methodDefinition_L.SetMethod].Add(new MethodWeavingConfiguration
                                {
                                    Type = methodMatch.Aspect.Type,
                                    After = methodMatch.Aspect.AfterPropertySet,
                                    Before = methodMatch.Aspect.BeforePropertySet,
                                    OnException = methodMatch.Aspect.OnException,
                                });
                        }
                    }
                }
            }

            return
                configurations.Select(
                    configuration_L =>
                    new AroundMethodWeaver(configuration_L.Value, configuration_L.Key,
                                           weave_P => new MethodWeaver(weave_P))).Cast<IWeaveable>();
        }
    }
}