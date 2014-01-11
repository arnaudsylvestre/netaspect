using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Helpers.IL
{
   public static class InstructionExtensions
   {
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