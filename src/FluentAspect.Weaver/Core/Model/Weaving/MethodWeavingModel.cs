using System.Collections.Generic;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.Engine;

namespace NetAspect.Weaver.Core.Model.Weaving
{
    public class MethodWeavingModel
    {
        public readonly Dictionary<Instruction, List<IAroundInstructionWeaver>> Instructions = new Dictionary<Instruction, List<IAroundInstructionWeaver>>();

        public void AddAroundInstructionWeaver(Instruction instruction, IAroundInstructionWeaver weaver)
        {
            if (!Instructions.ContainsKey(instruction))
                Instructions.Add(instruction, new List<IAroundInstructionWeaver>());
            Instructions[instruction].Add(weaver);
        }

        public AroundMethodWeavingModel Method { get; set; }

    }
}