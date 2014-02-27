using System;
using System.Linq;
using FluentAspect.Weaver.Core.Model;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.V2
{
    public class CallMethodInstructionWeavingModelFiller : IWeavingModelFiller
    {
        public void FillWeavingModel(MethodDefinition method, NetAspectDefinition aspect, WeavingModel weavingModel)
        {
            if (method.Body == null)
                return;
            if (!method.Body.Instructions.Any(instruction_L => InstructionCompliant(instruction_L, aspect, method)))
                return;
        }

        private bool InstructionCompliant(Instruction instruction, NetAspectDefinition aspect, MethodDefinition method)
        {
            if (instruction.OpCode == OpCodes.Call || instruction.OpCode == OpCodes.Calli ||
                instruction.OpCode == OpCodes.Callvirt)
            {
                var methodReference = instruction.Operand as MethodReference;

                var aspectType = method.Module.Import(aspect.Type);
                var compliant = methodReference.Resolve().CustomAttributes.Any(customAttribute_L => customAttribute_L.AttributeType.FullName == aspectType.FullName);
                return compliant;
            }
            return false;
        }
    }
}