using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.Weaver.Engine;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weaver.Fillers
{
    public class CallUpdateFieldInstructionWeavingModelFiller : IWeavingModelFiller
    {
        public bool CanHandle(NetAspectDefinition aspect)
        {
            return aspect.BeforeUpdateField.Method != null ||
                   aspect.AfterUpdateField.Method != null;
        }

        public void FillWeavingModel(MethodDefinition method, NetAspectDefinition aspect, WeavingModel weavingModel)
        {
            if (method.Body == null)
                return;
            foreach (var instruction in method.Body.Instructions)
            {
                if (IsFieldCall(instruction, aspect, method))
                {
                    weavingModel.AddUpdateFieldCallWeavingModel(method, instruction, aspect, aspect.BeforeUpdateField,
                                                             aspect.AfterUpdateField);

                }
            }
        }

        private static bool IsFieldCall(Instruction instruction, NetAspectDefinition aspect, MethodDefinition method)
        {
            if (instruction.OpCode == OpCodes.Stsfld || instruction.OpCode == OpCodes.Stfld)
            {
                var fieldReference = instruction.Operand as FieldReference;

                return AspectApplier.CanApply(fieldReference.Resolve(), aspect);
            }
            return false;
        }
    }
}