using System.Collections.Generic;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.Engine;
using NetAspect.Weaver.Core.Weaver.Engine.Instructions;

namespace NetAspect.Weaver.Core.Model.Weaving
{
    public class MethodWeavingModel
    {
        public readonly Dictionary<Instruction, List<AroundInstructionWeaver>> Instructions = new Dictionary<Instruction, List<AroundInstructionWeaver>>();

        public void AddAroundInstructionWeaver(Instruction instruction, AroundInstructionWeaver weaver)
        {
            if (!Instructions.ContainsKey(instruction))
                Instructions.Add(instruction, new List<AroundInstructionWeaver>());
            Instructions[instruction].Add(weaver);
        }

        public AroundMethodWeavingModel Method = new AroundMethodWeavingModel();

    }
}