using System.Collections.Generic;
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
       public static void AddRange(this IList<Instruction> instructions, IEnumerable<Instruction> toAdd)
       {
           foreach (var instruction in toAdd)
           {
               instructions.Add(instruction);
           }
       }
   }
}