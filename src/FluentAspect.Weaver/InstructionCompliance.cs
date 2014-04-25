using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model.Aspect;

namespace NetAspect.Weaver
{
    public static class InstructionCompliance
    {
        public static bool IsGetFieldInstruction(Instruction instruction, NetAspectDefinition aspect,
                                                 MethodDefinition method)
        {
            return instruction.OpCode == OpCodes.Ldfld || instruction.OpCode == OpCodes.Ldflda ||
                   instruction.OpCode == OpCodes.Ldsfld ||
                   instruction.OpCode == OpCodes.Ldsflda;
        }
    }
}