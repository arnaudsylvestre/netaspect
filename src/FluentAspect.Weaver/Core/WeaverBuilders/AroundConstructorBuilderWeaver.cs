using System.Collections.Generic;
using System.Linq;
using FluentAspect.Weaver.Core.Configuration;
using FluentAspect.Weaver.Core.Weavers.Constructors;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.WeaverBuilders
{
    public class AroundConstructorBuilderWeaver : IWeaverBuilder
    {
        public IEnumerable<IWeaveable> BuildWeavers(WeavingConfiguration configuration)
        {
            var weavers = new List<IWeaveable>();
            foreach (MethodMatch methodMatch in configuration.Constructors)
            {
                foreach (var assemblyDefinition in methodMatch.AssembliesToScan)
                {
                    List<MethodDefinition> methods = assemblyDefinition.GetAllMethods();
                    weavers.AddRange((from methodDefinition in methods
                                      where methodMatch.Matcher(new MethodDefinitionAdapter(methodDefinition))
                                      select new AroundConstructorWeaver(methodMatch.MethodWeavingInterceptors, methodDefinition))
                                         .Cast<IWeaveable>());
                    
                }
            }
            return weavers;
        }
    }
}