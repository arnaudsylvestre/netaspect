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


    }

    public class MethodWeavingModel
    {
        public readonly Dictionary<Instruction, List<IAroundInstructionWeaver>> Instructions = new Dictionary<Instruction, List<IAroundInstructionWeaver>>();

        public void AddAroundInstructionWeaver(Instruction instruction, IAroundInstructionWeaver weaver)
        {
            if (!Instructions.ContainsKey(instruction))
                Instructions.Add(instruction, new List<IAroundInstructionWeaver>());
            Instructions[instruction].Add(weaver);
        }

        public MethodWeavingModel()
        {
            Method = new AroundMethodWeavingModel();
        }

        public AroundMethodWeavingModel Method { get; private set; }

        public bool IsEmpty
        {
            get
            {
                return Method.Afters.Count == 0 &&
                       Method.Befores.Count == 0 &&
                       Method.OnExceptions.Count == 0 &&
                       Method.OnFinallys.Count == 0 &&
                       Instructions.Count == 0;
            }
        }
    }
}