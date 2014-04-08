using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Core.Helpers;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Detectors.Helpers;

namespace NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Field
{
    public class CallUpdateFieldInstructionWeavingDetector : IWeavingDetector
    {
        public bool CanHandle(NetAspectDefinition aspect)
        {
            return aspect.BeforeUpdateField.Method != null ||
                   aspect.AfterUpdateField.Method != null;
        }

        public void DetectWeavingModel(MethodDefinition method, NetAspectDefinition aspect, WeavingModel weavingModel)
        {
            if (method.Body == null)
                return;
            foreach (var instruction in method.ExtractRealInstructions())
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