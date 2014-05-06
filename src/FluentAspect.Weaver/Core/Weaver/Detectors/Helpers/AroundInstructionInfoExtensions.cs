using Mono.Cecil;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Detectors.Helpers
{
    public static class AroundInstructionInfoExtensions
    {
        public static FieldDefinition GetOperandAsField(this InstructionWeavingInfo instructionWeaving_P)
        {
            return (instructionWeaving_P.Instruction.Operand as FieldReference).Resolve();
        }
        public static MethodDefinition GetOperandAsMethod(this InstructionWeavingInfo instructionWeaving_P)
        {
            return (instructionWeaving_P.Instruction.Operand as MethodReference).Resolve();
        }

        
    }
}