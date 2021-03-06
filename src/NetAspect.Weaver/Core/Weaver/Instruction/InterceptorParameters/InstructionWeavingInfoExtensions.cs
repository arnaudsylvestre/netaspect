using Mono.Cecil;
using NetAspect.Weaver.Helpers.Mono.Cecil.IL;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model
{
   public static class InstructionWeavingInfoExtensions
   {
      public static FieldDefinition GetOperandAsField(this InstructionWeavingInfo instructionWeaving_P)
      {
         return (instructionWeaving_P.Instruction.Operand as FieldReference).Resolve();
      }

      public static MethodDefinition GetOperandAsMethod(this InstructionWeavingInfo instructionWeaving_P)
      {
         return instructionWeaving_P.Instruction.GetOperandAsMethod();
      }
   }
}
