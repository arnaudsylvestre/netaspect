using System.Collections.Generic;
using Mono.Cecil;
using NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Engine;
using NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Model;
using NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving.Engine;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Helpers
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