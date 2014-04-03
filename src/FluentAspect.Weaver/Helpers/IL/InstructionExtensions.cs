using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace NetAspect.Weaver.Helpers.IL
{
    public static class InstructionExtensions
    {

        public static bool IsACallInstruction(this Instruction instruction)
        {
            return instruction.OpCode == OpCodes.Call ||
                instruction.OpCode == OpCodes.Calli ||
                instruction.OpCode == OpCodes.Callvirt;
        }
        public static bool IsAnAccessField(this Instruction instruction)
        {
            return instruction.OpCode == OpCodes.Ldsfld ||
                instruction.OpCode == OpCodes.Ldfld ||
                instruction.OpCode == OpCodes.Ldflda ||
                instruction.OpCode == OpCodes.Ldsflda;
        }
        public static bool IsAnUpdateField(this Instruction instruction)
        {
            return instruction.OpCode == OpCodes.Stsfld ||
                instruction.OpCode == OpCodes.Stfld;
        }
        public static MethodDefinition GetCalledMethod(this Instruction instruction)
        {
            return ((MethodReference)instruction.Operand).Resolve();
        }

        public static SequencePoint GetLastSequencePoint(this Instruction instruction)
        {
            if (instruction == null)
                return null;
            if (instruction.SequencePoint != null)
                return instruction.SequencePoint;
            return instruction.Previous.GetLastSequencePoint();
        }

        public static void AddRange(this IList<Instruction> instructions, IEnumerable<Instruction> toAdd)
        {
            foreach (Instruction instruction in toAdd)
            {
                instructions.Add(instruction);
            }
        }
    }
}