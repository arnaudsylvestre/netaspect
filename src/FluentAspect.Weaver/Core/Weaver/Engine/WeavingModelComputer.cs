using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Assemblies;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Detectors;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;
using NetAspect.Weaver.Helpers;

namespace NetAspect.Weaver.Core.Weaver.Engine
{
   public class WeavingModelComputer
   {
      private readonly IAspectChecker _aspectChecker;
      private readonly IAspectFinder _aspectFinder;
      private readonly AspectBuilder aspectBuilder;
      private readonly List<ICallWeavingDetector> callWeavingDetector;
      private readonly List<IMethodWeavingDetector> methodWeavingDetector;

      public WeavingModelComputer(IAspectFinder aspectFinder_P,
         IAspectChecker aspectChecker_P,
         List<ICallWeavingDetector> callWeavingDetector,
         List<IMethodWeavingDetector> methodWeavingDetector,
         AspectBuilder aspectBuilder)
      {
         _aspectFinder = aspectFinder_P;
         _aspectChecker = aspectChecker_P;
         this.callWeavingDetector = callWeavingDetector;
         this.methodWeavingDetector = methodWeavingDetector;
         this.aspectBuilder = aspectBuilder;
      }

      public MethodsWeavingModel ComputeWeavingModels(Type[] typesP_L,
         Type[] filter,
         AssemblyPool assemblyPool,
         ErrorHandler errorHandler)
      {
         List<NetAspectDefinition> aspects = _aspectFinder.Find(typesP_L);
         aspects.ForEach(aspect => _aspectChecker.Check(aspect, errorHandler));
         IEnumerable<Assembly> assembliesToWeave = aspects.GetAssembliesToWeave(typesP_L[0].Assembly);
         return ComputeWeavingModels(assembliesToWeave, filter, assemblyPool, aspects);
      }

      private MethodsWeavingModel ComputeWeavingModels(IEnumerable<Assembly> assembliesToWeave,
         Type[] filter,
         AssemblyPool assemblyPool,
         IEnumerable<NetAspectDefinition> aspects)
      {
         var weavingModels = new MethodsWeavingModel();
         assemblyPool.Add(assembliesToWeave);
         foreach (Assembly assembly_L in assembliesToWeave)
         {
            foreach (
               MethodDefinition method in
                  assemblyPool.GetAssemblyDefinition(assembly_L)
                     .GetAllMethodsWithBody(filter))
            {
               foreach (NetAspectDefinition aspect_L in aspects)
               {
                  methodWeavingDetector.ForEach(model => weavingModels.Add(method, model.DetectWeavingModel(method, aspect_L), aspect_L, aspectBuilder));
                  foreach (Instruction instruction in method.Body.Instructions)
                  {
                     callWeavingDetector.ForEach(
                        model => weavingModels.Add(
                           method,
                           instruction,
                           model.DetectWeavingModel(method, instruction, aspect_L),
                           aspect_L,
                           aspectBuilder));
                  }
               }
            }
         }
         return weavingModels;
      }

      public interface IAspectChecker
      {
         void Check(NetAspectDefinition aspect_P, ErrorHandler errorHandler_P);
      }

      public interface IAspectFinder
      {
         List<NetAspectDefinition> Find(IEnumerable<Type> types);
      }
   }
}
