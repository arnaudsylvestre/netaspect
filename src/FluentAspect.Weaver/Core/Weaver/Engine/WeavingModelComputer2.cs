using System;
using System.Collections.Generic;
using Mono.Cecil;
using NetAspect.Weaver.Core.Assemblies;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;

namespace NetAspect.Weaver.Core.Weaver.Engine
{
   public class WeavingModelComputer2
   {
      public interface IAspectFinder
      {
         List<NetAspectDefinition> Find(IEnumerable<Type> types);
      }

      public interface IAspectChecker
      {
         void Check(NetAspectDefinition aspect_P, ErrorHandler errorHandler_P);
      }

      private readonly WeavingModelComputer _weavingModelComputer2;
      private IAspectFinder aspectFinder;
      private IAspectChecker aspectChecker;

      public WeavingModelComputer2(WeavingModelComputer weavingModelComputer2_P, IAspectFinder aspectFinder_P)
      {
         _weavingModelComputer2 = weavingModelComputer2_P;
         aspectFinder = aspectFinder_P;
      }

      public Dictionary<MethodDefinition, WeavingModel> ComputeWeavingModels(Type[] typesP_L, Type[] filter, AssemblyPool assemblyPool, ErrorHandler errorHandler)
      {
         var aspects = aspectFinder.Find(typesP_L);
         aspects.ForEach(aspect => aspectChecker.Check(aspect, errorHandler));
         var assembliesToWeave = aspects.GetAssembliesToWeave(typesP_L[0].Assembly);
         return _weavingModelComputer2.ComputeWeavingModels(assembliesToWeave, filter, assemblyPool, aspects);
      }
   }
}
