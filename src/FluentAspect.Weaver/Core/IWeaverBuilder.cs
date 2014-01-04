using System.Collections.Generic;
using FluentAspect.Weaver.Core.Configuration;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core
{
    public interface IWeaverBuilder
    {
        IEnumerable<IWeaveable> BuildWeavers(AssemblyDefinition assemblyDefinition, WeavingConfiguration configuration);
    }
}