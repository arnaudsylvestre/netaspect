using System.Collections.Generic;
using System.Linq;
using FluentAspect.Weaver.Core.Configuration;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Weavers.Methods;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.WeaverBuilders
{
   public class AroundMethodBuilderWeaver : IWeaverBuilder
   {
       public IEnumerable<IWeaveable> BuildWeavers(AssemblyDefinition assemblyDefinition, WeavingConfiguration configuration, ErrorHandler errorHandler)
      {
         List<IWeaveable> weavers = new List<IWeaveable>();
         var methods = assemblyDefinition.GetAllMethods();
         foreach (var methodMatch in configuration.Methods)
         {
            weavers.AddRange((from methodDefinition in methods
                              where methodMatch.Matcher(new MethodDefinitionAdapter(methodDefinition))
                              select new AroundMethodWeaver(methodMatch.InterceptorTypes, methodDefinition))
                              .Cast<IWeaveable>());
         }
         return weavers;
      }
   }
}