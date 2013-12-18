using System.Collections.Generic;
using FluentAspect.Core;
using FluentAspect.Weaver.Core.Errors;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core
{
   public interface IWeaverBuilder
   {
      IEnumerable<IWeaveable> BuildWeavers(AssemblyDefinition assemblyDefinition, WeavingConfiguration configuration, ErrorHandler errorHandler);
   }
}