using System.Collections.Generic;
using FluentAspect.Weaver.Core.Configuration;
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

        public IEnumerable<IWeaveable> BuildWeavers(WeavingConfiguration configuration)
        {
            var weavables = new List<IWeaveable>();
            foreach (IWeaverBuilder weaverBuilder in builders)
            {
                weavables.AddRange(weaverBuilder.BuildWeavers(configuration));
            }
            return weavables;
        }
    }
}