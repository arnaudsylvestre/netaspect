using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.Engine.Instructions;

namespace NetAspect.Weaver.Core.Model.Weaving
{
   public class WeavingSession
   {
      public readonly Dictionary<MethodDefinition, WeavingMethodSession> weavingModels = new Dictionary<MethodDefinition, WeavingMethodSession>();


      public void Add(MethodDefinition method, IEnumerable<AspectInstanceForMethodWeaving> detectWeavingModels)
      {
          if (detectWeavingModels == null)
            return;
          var methodWeavingModel = GetMethodWeavingModel(method);
          methodWeavingModel.Method.AddRange(detectWeavingModels.ToList());
      }

      private WeavingMethodSession GetMethodWeavingModel(MethodDefinition method)
      {
         if (!weavingModels.ContainsKey(method))
            weavingModels.Add(method, new WeavingMethodSession());
         return weavingModels[method];
      }

      public void Add(MethodDefinition method, Instruction instruction, IEnumerable<AspectInstanceForInstruction> aroundInstructionWeaver)
      {
         if (aroundInstructionWeaver == null)
            return;
         GetMethodWeavingModel(method).AddAroundInstructionWeaver(instruction, aroundInstructionWeaver);
      }
   }
}
