using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Weaver.Engine;
using NetAspect.Weaver.Core.Weaver.Engine.Instructions;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Model.Weaving
{
    public class MethodsWeavingModel
    {
        public Dictionary<MethodDefinition, MethodWeavingModel> weavingModels = new Dictionary<MethodDefinition, MethodWeavingModel>();

        public void Add(MethodDefinition method, MethodWeavingModel model)
        {
            weavingModels.Add(method, model);
        }


        public void Add(MethodDefinition method, AroundMethodWeavingModel detectWeavingModel, NetAspectDefinition aspect, AspectBuilder aspectBuilder)
        {
            if (detectWeavingModel == null)
                return;
            var methodWeavingModel = GetMethodWeavingModel(method, aspect, aspectBuilder);
            methodWeavingModel.Method.Model.Befores.AddRange(detectWeavingModel.Befores);
            methodWeavingModel.Method.Model.Afters.AddRange(detectWeavingModel.Afters);
            methodWeavingModel.Method.Model.OnExceptions.AddRange(detectWeavingModel.OnExceptions);
            methodWeavingModel.Method.Model.OnFinallys.AddRange(detectWeavingModel.OnFinallys);
        }

        private MethodWeavingModel GetMethodWeavingModel(MethodDefinition method, NetAspectDefinition aspect, AspectBuilder aspectBuilder)
        {
            if (!weavingModels.ContainsKey(method))
                weavingModels.Add(method, new MethodWeavingModel(method, aspect, aspectBuilder));
            var methodWeavingModel = weavingModels[method];
            return methodWeavingModel;
        }

        public void Add(MethodDefinition method, Instruction instruction, AroundInstructionWeaver aroundInstructionWeaver, NetAspectDefinition aspect, AspectBuilder aspectBuilder)
        {
            if (aroundInstructionWeaver == null)
                return;
            GetMethodWeavingModel(method, aspect, aspectBuilder).AddAroundInstructionWeaver(instruction, aroundInstructionWeaver);;
        }
    }
}