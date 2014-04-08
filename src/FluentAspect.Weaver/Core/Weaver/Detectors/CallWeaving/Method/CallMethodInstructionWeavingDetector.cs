using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model;
using NetAspect.Weaver.Core.Weaver.Engine;

namespace NetAspect.Weaver.Core.Weaver.Fillers
{
    public class CallMethodInstructionWeavingDetector : IWeavingDetector
    {
        public bool CanHandle(NetAspectDefinition aspect)
        {
            return aspect.BeforeCallMethod.Method != null ||
                aspect.AfterCallMethod.Method != null ||
                aspect.AfterRaiseEvent.Method != null ||
                aspect.BeforeRaiseEvent.Method != null;
        }

        public void FillWeavingModel(MethodDefinition method, NetAspectDefinition aspect, WeavingModel weavingModel)
        {
            if (method.Body == null)
                return;
            foreach (Instruction instruction in method.Body.Instructions)
            {
                if (IsMethodCall(instruction, aspect, method))
                {
                    weavingModel.AddMethodCallWeavingModel(method, instruction, aspect, aspect.BeforeCallMethod,
                                                           aspect.AfterCallMethod);
                }
            }
        }

        private static bool IsMethodCall(Instruction instruction, NetAspectDefinition aspect, MethodDefinition method)
        {
            if (instruction.OpCode == OpCodes.Call || instruction.OpCode == OpCodes.Calli ||
                instruction.OpCode == OpCodes.Callvirt)
            {
                var methodReference = instruction.Operand as MethodReference;

                TypeReference aspectType = method.Module.Import(aspect.Type);
                bool compliant =
                    methodReference.Resolve()
                                   .CustomAttributes.Any(
                                       customAttribute_L =>
                                       customAttribute_L.AttributeType.FullName == aspectType.FullName);
                return compliant;
            }
            return false;
        }
    }
}