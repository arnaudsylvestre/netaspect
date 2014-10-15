using System.Collections.Generic;
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


      public void Add(MethodDefinition method, MethodWeavingAspectInstance detectWeavingAspectInstance, NetAspectDefinition aspect, AspectBuilder aspectBuilder)
      {
          if (detectWeavingAspectInstance == null)
            return;
          {
              MethodWeavingModel methodWeavingModel = GetMethodWeavingModel(method, aspect, aspectBuilder);
              methodWeavingModel.Method.AspectInstance.BeforeConstructorBaseCalls.AddRange(detectWeavingAspectInstance.BeforeConstructorBaseCalls);
              methodWeavingModel.Method.AspectInstance.Befores.AddRange(detectWeavingAspectInstance.Befores);
              methodWeavingModel.Method.AspectInstance.Afters.AddRange(detectWeavingAspectInstance.Afters);
              methodWeavingModel.Method.AspectInstance.OnExceptions.AddRange(detectWeavingAspectInstance.OnExceptions);
              methodWeavingModel.Method.AspectInstance.OnFinallys.AddRange(detectWeavingAspectInstance.OnFinallys);
          }
      }

      private MethodWeavingModel GetMethodWeavingModel(MethodDefinition method, NetAspectDefinition aspect, AspectBuilder aspectBuilder)
      {
         if (!weavingModels.ContainsKey(method))
            weavingModels.Add(method, new MethodWeavingModel(aspect));
         return weavingModels[method];
      }

      public void Add(MethodDefinition method, Instruction instruction, AroundInstructionWeaver aroundInstructionWeaver, NetAspectDefinition aspect, AspectBuilder aspectBuilder)
      {
         if (aroundInstructionWeaver == null)
            return;
         GetMethodWeavingModel(method, aspect, aspectBuilder).AddAroundInstructionWeaver(instruction, aroundInstructionWeaver);
         ;
      }
   }
}
