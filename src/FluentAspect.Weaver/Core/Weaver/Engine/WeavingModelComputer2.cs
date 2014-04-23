using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Assemblies;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;

namespace NetAspect.Weaver.Core.Weaver.Engine
{
   public class WeavingModelComputer2
   {
      private readonly WeavingModelComputer _weavingModelComputer2;

      public WeavingModelComputer2(WeavingModelComputer weavingModelComputer2_P)
      {
         _weavingModelComputer2 = weavingModelComputer2_P;
      }

      public Dictionary<MethodDefinition, WeavingModel> ComputeWeavingModels(Type[] typesP_L, Type[] filter, AssemblyPool assemblyPool, ErrorHandler errorHandler)
      {
         List<NetAspectDefinition> aspects = NetAspectDefinitionExtensions.FindAspects(typesP_L);
         CheckAspects(aspects, errorHandler);
         IEnumerable<Assembly> assembliesToWeave = aspects.GetAssembliesToWeave(typesP_L[0].Assembly);
         Dictionary<MethodDefinition, WeavingModel> weavingModels =
             _weavingModelComputer2.ComputeWeavingModels(assembliesToWeave, filter, assemblyPool, aspects);
         return weavingModels;
      }

      private void CheckAspects(IEnumerable<NetAspectDefinition> aspects, ErrorHandler errorHandler)
      {
         foreach (var aspect in aspects)
         {
            aspect.FieldSelector.Check(errorHandler);
            aspect.PropertySelector.Check(errorHandler);
         }
      } 
   }
}