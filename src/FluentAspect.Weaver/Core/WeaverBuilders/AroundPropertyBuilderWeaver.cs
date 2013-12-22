using System.Collections.Generic;
using System.Linq;
using FluentAspect.Core;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Weavers.Properties;
using FluentAspect.Weaver.Weavers.Methods;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core
{
   public class AroundPropertyBuilderWeaver : IWeaverBuilder
   {
      public IEnumerable<IWeaveable> BuildWeavers(AssemblyDefinition assemblyDefinition, WeavingConfiguration configuration, ErrorHandler errorHandler)
      {
         List<IWeaveable> weavers = new List<IWeaveable>();
         var properties = assemblyDefinition.GetAllProperties();
         foreach (var methodMatch in configuration.Properties)
         {
            weavers.AddRange((from methodDefinition in properties
                              where methodMatch.Matcher(new PropertyDefinitionAdapter(methodDefinition))
                              select new AroundPropertyWeaver(methodMatch.InterceptorTypes, methodDefinition))
                              .Cast<IWeaveable>());
         }
         return weavers;
      }
   }
}