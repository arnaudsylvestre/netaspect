using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Assemblies;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Detectors;
using NetAspect.Weaver.Helpers;

namespace NetAspect.Weaver.Core.Weaver.Engine
{
   public class WeavingModelComputer
   {
      public interface IAspectFinder
      {
         List<NetAspectDefinition> Find(IEnumerable<Type> types);
      }

      public interface IAspectChecker
      {
         void Check(NetAspectDefinition aspect_P, ErrorHandler errorHandler_P);
      }

      private readonly IWeavingDetector _weavingDetector;
      private readonly IAspectFinder _aspectFinder;
      private readonly IAspectChecker _aspectChecker;

      public WeavingModelComputer(IWeavingDetector weavingDetector, IAspectFinder aspectFinder_P, IAspectChecker aspectChecker_P)
      {
         _weavingDetector = weavingDetector;
         _aspectFinder = aspectFinder_P;
         _aspectChecker = aspectChecker_P;
      }

      public Dictionary<MethodDefinition, WeavingModel> ComputeWeavingModels(Type[] typesP_L, Type[] filter, AssemblyPool assemblyPool, ErrorHandler errorHandler)
      {
         var aspects = _aspectFinder.Find(typesP_L);
         aspects.ForEach(aspect => _aspectChecker.Check(aspect, errorHandler));
         var assembliesToWeave = aspects.GetAssembliesToWeave(typesP_L[0].Assembly);
         return ComputeWeavingModels(assembliesToWeave, filter, assemblyPool, aspects);
      }

       private Dictionary<MethodDefinition, WeavingModel> ComputeWeavingModels(IEnumerable<Assembly> assembliesToWeave, Type[] filter, AssemblyPool assemblyDefinitionProvider, IEnumerable<NetAspectDefinition> aspects)
      {
         var weavingModels = new Dictionary<MethodDefinition, WeavingModel>();
         assemblyDefinitionProvider.Add(assembliesToWeave);
         foreach (var assembly_L in assembliesToWeave)
         {
            foreach (var method in assemblyDefinitionProvider.GetAssemblyDefinition(assembly_L).GetAllMethods(filter))
            {
               var model = new WeavingModel();
                foreach (var aspect_L in aspects)
                    _weavingDetector.DetectWeavingModel(method, aspect_L, model);
                if (!model.IsEmpty)
                  weavingModels.Add(method, model);
            }
         }
         return weavingModels;
      }
   }
}
