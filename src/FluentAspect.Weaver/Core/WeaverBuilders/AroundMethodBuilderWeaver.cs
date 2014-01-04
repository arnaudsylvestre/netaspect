using System.Collections.Generic;
using System.Linq;
using FluentAspect.Weaver.Core.Configuration;
using FluentAspect.Weaver.Core.Weavers.Methods;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.WeaverBuilders
{
    public class AroundMethodBuilderWeaver : IWeaverBuilder
    {
        public IEnumerable<IWeaveable> BuildWeavers(AssemblyDefinition assemblyDefinition,
                                                    WeavingConfiguration configuration)
        {
            var weavers = new List<IWeaveable>();
            List<MethodDefinition> methods = assemblyDefinition.GetAllMethods();
            foreach (MethodMatch methodMatch in configuration.Methods)
            {
                weavers.AddRange((from methodDefinition in methods
                                  where methodMatch.Matcher(new MethodDefinitionAdapter(methodDefinition))
                                  select new AroundMethodWeaver(methodMatch.Interceptors, methodDefinition))
                                     .Cast<IWeaveable>());
            }
            return weavers;
        }
    }
}