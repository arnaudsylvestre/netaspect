using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.Engine;
using NetAspect.Weaver.Core.Weaver.Engine.Instructions;

namespace NetAspect.Weaver.Core.Model.Weaving
{
    public class MethodsWeavingModel
    {
        public Dictionary<MethodDefinition, MethodWeavingModel> weavingModels = new Dictionary<MethodDefinition, MethodWeavingModel>();

        public void Add(MethodDefinition method, MethodWeavingModel model)
        {
            weavingModels.Add(method, model);
        }


        public void Add(MethodDefinition method, AroundMethodWeavingModel detectWeavingModel)
        {
            if (detectWeavingModel == null)
                return;
            var methodWeavingModel = GetMethodWeavingModel(method);
            methodWeavingModel.Method.Befores.AddRange(detectWeavingModel.Befores);
            methodWeavingModel.Method.Afters.AddRange(detectWeavingModel.Afters);
            methodWeavingModel.Method.OnExceptions.AddRange(detectWeavingModel.OnExceptions);
            methodWeavingModel.Method.OnFinallys.AddRange(detectWeavingModel.OnFinallys);
        }

        private MethodWeavingModel GetMethodWeavingModel(MethodDefinition method)
        {
            if (!weavingModels.ContainsKey(method))
                weavingModels.Add(method, new MethodWeavingModel());
            var methodWeavingModel = weavingModels[method];
            return methodWeavingModel;
        }

        public void Add(MethodDefinition method, Instruction instruction, AroundInstructionWeaver aroundInstructionWeaver)
        {
            if (aroundInstructionWeaver == null)
                return;
            GetMethodWeavingModel(method).AddAroundInstructionWeaver(instruction, aroundInstructionWeaver);;
        }
    }
}