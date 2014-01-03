using System.Collections.Generic;
using FluentAspect.Weaver.Core.Configuration;
using FluentAspect.Weaver.Core.Errors;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.WeaverBuilders
{
    internal class MultiWeaverBuilder : IWeaverBuilder
    {
        private readonly IEnumerable<IWeaverBuilder> builders;

        public MultiWeaverBuilder(params IWeaverBuilder[] builders)
        {
            this.builders = builders;
        }

        public IEnumerable<IWeaveable> BuildWeavers(AssemblyDefinition assemblyDefinition,
                                                    WeavingConfiguration configuration, ErrorHandler errorHandler)
        {
            var weavables = new List<IWeaveable>();
            foreach (IWeaverBuilder weaverBuilder in builders)
            {
                weavables.AddRange(weaverBuilder.BuildWeavers(assemblyDefinition, configuration, errorHandler));
            }
            return weavables;
        }
    }
}