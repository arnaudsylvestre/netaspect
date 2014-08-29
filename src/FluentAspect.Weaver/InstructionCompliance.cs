using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Core.Helpers;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver
{
    public static class InstructionCompliance
    {
        public static bool IsGetFieldInstruction(Instruction instruction, NetAspectDefinition aspect,
                                                 MethodDefinition method)
        {
            return instruction.OpCode == OpCodes.Ldfld || instruction.OpCode == OpCodes.Ldflda ||
                   instruction.OpCode == OpCodes.Ldsfld ||
                   instruction.OpCode == OpCodes.Ldsflda;
        }
        public static bool IsUpdateFieldInstruction(Instruction instruction, NetAspectDefinition aspect, MethodDefinition method)
        {
            return instruction.OpCode == OpCodes.Stsfld || instruction.OpCode == OpCodes.Stfld;
        }

        public static bool IsCallMethodInstruction(Instruction instruction, NetAspectDefinition aspect, MethodDefinition method)
        {
           return instruction.IsACallInstruction() && !(instruction.Operand as MethodReference).Resolve().IsConstructor; ;
        }
        public static bool IsCallConstructorInstruction(Instruction instruction, NetAspectDefinition aspect, MethodDefinition method)
        {
           return instruction.IsACallInstruction() && (instruction.Operand as MethodReference).Resolve().IsConstructor; ;
        }
        public static bool IsGetPropertyCall(Instruction instruction, NetAspectDefinition aspect, MethodDefinition method)
        {

            if (instruction.IsACallInstruction())
            {
                var calledMethod = (instruction.Operand as MethodReference).Resolve();
                var property_L = calledMethod.GetPropertyForGetter();
                if (property_L != null)
                    return true;


            }
            return false;
        }

        public static bool IsSetPropertyCall(Instruction instruction, NetAspectDefinition aspect, MethodDefinition method)
        {

            if (instruction.IsACallInstruction())
            {
                var calledMethod = (instruction.Operand as MethodReference).Resolve();
                var property_L = calledMethod.GetPropertyForSetter();
                if (property_L != null)
                    return true;


            }
            return false;
        }
    }
}