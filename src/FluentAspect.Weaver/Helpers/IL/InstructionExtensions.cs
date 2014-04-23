using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Core.Helpers;

namespace NetAspect.Weaver.Helpers.IL
{
    public static class InstructionExtensions
    {
       public static bool IsAnUpdateField(this Instruction instruction)
       {
          return instruction.OpCode == OpCodes.Stsfld ||
              instruction.OpCode == OpCodes.Stfld;
       }
       public static bool IsAnUpdatePropertyCall(this Instruction instruction)
       {
          if (!instruction.IsACallInstruction())
             return false;
          var methodDefinition_L = ((MethodReference) instruction.Operand).Resolve();
          var property = methodDefinition_L.GetPropertyForSetter();
          return (property != null);
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
    }
}