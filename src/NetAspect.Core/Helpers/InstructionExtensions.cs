using Mono.Cecil;
using Mono.Cecil.Cil;

namespace NetAspect.Core.Helpers
{
    public static class InstructionExtensions
    {

        public static bool IsCallBaseConstructor(this Instruction instruction)
        {
            if (instruction.IsACallInstruction())
            {
                if (instruction.Operand is MethodReference)
                {
                    var methodReference = instruction.Operand as MethodReference;
                    return methodReference.Name == ".ctor";
                }
            }
            return false;
        }

        public static bool IsACallInstruction(this Instruction instruction)
        {
            return instruction.OpCode == OpCodes.Call ||
                   instruction.OpCode == OpCodes.Calli ||
                   instruction.OpCode == OpCodes.Callvirt;
        }

        public static bool IsANewInstruction(this Instruction instruction)
        {
            return instruction.OpCode == OpCodes.Newobj;
        }
    }
}