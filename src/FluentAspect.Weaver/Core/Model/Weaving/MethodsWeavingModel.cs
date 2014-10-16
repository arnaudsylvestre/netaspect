using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Weaver.Engine.Instructions;
using NetAspect.Weaver.Core.Weaver.Engine.Lifecycle;

namespace NetAspect.Weaver.Core.Model.Weaving
{
   public class MethodsWeavingModel
   {
      public readonly Dictionary<MethodDefinition, MethodWeavingModel> weavingModels = new Dictionary<MethodDefinition, MethodWeavingModel>();


      public void Add(MethodDefinition method, IEnumerable<AspectInstanceForMethodWeaving> detectWeavingModels, NetAspectDefinition aspect, AspectBuilder aspectBuilder)
      {
          if (detectWeavingModels == null)
            return;
          var methodWeavingModel = GetMethodWeavingModel(method, aspect, aspectBuilder);
          methodWeavingModel.Method.AddRange(detectWeavingModels.ToList());
      }

      private MethodWeavingModel GetMethodWeavingModel(MethodDefinition method, NetAspectDefinition aspect, AspectBuilder aspectBuilder)
      {
         if (!weavingModels.ContainsKey(method))
            weavingModels.Add(method, new MethodWeavingModel(aspect));
         return weavingModels[method];
      }

      public void Add(MethodDefinition method, Instruction instruction, IEnumerable<AroundInstructionWeaver> aroundInstructionWeaver, NetAspectDefinition aspect, AspectBuilder aspectBuilder)
      {
         if (aroundInstructionWeaver == null)
            return;
         GetMethodWeavingModel(method, aspect, aspectBuilder).AddAroundInstructionWeaver(instruction, aroundInstructionWeaver);
      }
   }
}
