using System.Collections.Generic;
using Mono.Cecil.Cil;

namespace NetAspect.Core
{
    public class NetAspectWeavingMethod
    {
        public class InstructionIl
        {
            public List<Instruction> Before = new List<Instruction>();
            public List<Instruction> After = new List<Instruction>();
        }

        public List<VariableDefinition> Variables = new List<VariableDefinition>();
        public readonly List<Instruction> BeforeConstructorBaseCall = new List<Instruction>();
        public readonly List<Instruction> BeforeInstructions = new List<Instruction>();
        public List<Instruction> AfterInstructions = new List<Instruction>();
        public List<Instruction> OnExceptionInstructions = new List<Instruction>();
        public List<Instruction> OnFinallyInstructions = new List<Instruction>();
        public Dictionary<Instruction, InstructionIl> Instructions = new Dictionary<Instruction, InstructionIl>(); 
    }
}