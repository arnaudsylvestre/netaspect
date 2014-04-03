using System;
using Mono.Cecil.Cil;

namespace NetAspect.Weaver.Core.Weaver.Helpers
{
    public class InstructionFactory
    {
        public static Instruction Create(SequencePoint instructionP_P, Func<SequencePoint, int> provider)
        {
            return Instruction.Create(OpCodes.Ldc_I4,
                                      instructionP_P == null
                                          ? 0
                                          : provider(instructionP_P));
        }

        public static Instruction Create(SequencePoint instructionP_P, Func<SequencePoint, string> provider)
        {
            return Instruction.Create(OpCodes.Ldstr,
                                      instructionP_P == null
                                          ? null
                                          : provider(instructionP_P));
        }
    }
}