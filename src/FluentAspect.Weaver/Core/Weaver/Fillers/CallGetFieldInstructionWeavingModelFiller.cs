using System;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.V2.Model;
using Mono.Cecil;
using Mono.Cecil.Cil;
using FluentAspect.Weaver.Core.V2.Weaver.Engine;

namespace FluentAspect.Weaver.Core.V2.Weaver.Fillers
{
    public class CallGetFieldInstructionWeavingModelFiller : IWeavingModelFiller
    {
        public bool CanHandle(NetAspectDefinition aspect)
        {
            return aspect.BeforeGetField.Method != null ||
                   aspect.AfterGetField.Method != null;
        }

        public void FillWeavingModel(MethodDefinition method, NetAspectDefinition aspect, WeavingModel weavingModel)
        {
            if (method.Body == null)
                return;
            foreach (var instruction in method.Body.Instructions)
            {
                if (IsFieldCall(instruction, aspect, method))
                {
                    weavingModel.AddGetFieldCallWeavingModel(method, instruction, aspect, aspect.BeforeGetField,
                                                             aspect.AfterGetField);

                }
            }
        }

        private static bool IsFieldCall(Instruction instruction, NetAspectDefinition aspect, MethodDefinition method)
        {
            if (instruction.OpCode == OpCodes.Ldfld || instruction.OpCode == OpCodes.Ldflda ||
                instruction.OpCode == OpCodes.Ldsfld ||
                instruction.OpCode == OpCodes.Ldsflda)
            {
                var fieldReference = instruction.Operand as FieldReference;

                return AspectApplier.CanApply(fieldReference.Resolve(), aspect);
            }
            return false;
        }
    }
}