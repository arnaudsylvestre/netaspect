using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Core.Helpers;

namespace NetAspect.Core
{
    public static class InstructionsExtensions
    {
        public static List<Instruction> ExtractBeforeCallBaseConstructorInstructions(this IEnumerable<Instruction> instructions)
        {
            return instructions.Until(InstructionExtensions.IsCallBaseConstructor);
        }
        public static Instruction GetCallBaseConstructorInstructions(this IEnumerable<Instruction> instructions)
        {
            return instructions.First(InstructionExtensions.IsCallBaseConstructor);
        }
    }
}