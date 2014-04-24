using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.Engine;

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
            GetMethodWeavingModel(method).Method = detectWeavingModel;
        }

        private MethodWeavingModel GetMethodWeavingModel(MethodDefinition method)
        {
            if (!weavingModels.ContainsKey(method))
                weavingModels.Add(method, new MethodWeavingModel());
            var methodWeavingModel = weavingModels[method];
            return methodWeavingModel;
        }

        public void Add(MethodDefinition method, Instruction instruction, IAroundInstructionWeaver aroundInstructionWeaver)
        {
            if (aroundInstructionWeaver == null)
                return;
            GetMethodWeavingModel(method).AddAroundInstructionWeaver(instruction, aroundInstructionWeaver);;
        }
    }
}