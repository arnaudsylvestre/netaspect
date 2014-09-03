using System.Collections.Generic;
using System.Linq;
using Mono.Cecil.Cil;

namespace NetAspect.Core.Helpers
{
   public static class InstructionsExtensions
   {
      public static List<Instruction> ExtractBeforeCallBaseConstructorInstructions(this IEnumerable<Instruction> instructions)
      {
         return instructions.Until(InstructionExtensions.IsCallBaseConstructor);
      }

      public static Instruction GetCallBaseConstructorInstructions(this IEnumerable<Instruction> instructions)
      {
         return instructions.FirstOrDefault(InstructionExtensions.IsCallBaseConstructor);
      }
   }
}
