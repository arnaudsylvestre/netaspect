using System.Collections.Generic;
using System.Linq;
using FluentAspect.Core;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Weavers.Methods;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core
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
                              select new AroundMethodWeaver(methodMatch.AdviceName, methodDefinition))
                              .Cast<IWeaveable>());
         }
         return weavers;
      }
   }
}