using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAspect.Weaver.Core;
using Mono.Cecil;

namespace FluentAspect.Weaver.CF.Core.WeaverBuilders
{
    class MultiWeaverBuilder : IWeaverBuilder
    {
        private IEnumerable<IWeaverBuilder> builders;

        public MultiWeaverBuilder(params IWeaverBuilder[] builders)
        {
            this.builders = builders;
        }

        public IEnumerable<IWeaveable> BuildWeavers(AssemblyDefinition assemblyDefinition, WeavingConfiguration configuration)
        {
            var weavables = new List<IWeaveable>();
            foreach (var weaverBuilder in builders)
            {
                weavables.AddRange(weaverBuilder.BuildWeavers(assemblyDefinition, configuration));
            }
            return weavables;
        }
    }
}
