using Mono.Cecil;
using NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving;

namespace NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Engine
{
    public static class AroundInstructionInfoExtensions
    {
        public static FieldDefinition GetOperandAsField(this AroundInstructionInfo aroundInstruction)
        {
            return (aroundInstruction.Instruction.Operand as FieldReference).Resolve();
        }
        public static MethodDefinition GetOperandAsMethod(this AroundInstructionInfo aroundInstruction)
        {
            return (aroundInstruction.Instruction.Operand as MethodReference).Resolve();
        }

        public static ParameterConfigurationForInstruction AddPossibleParameter(this AroundInstructionInfo aroundInstruction, string parameterName)
        {
            return new ParameterConfigurationForInstruction(aroundInstruction, aroundInstruction.Generator.Add(parameterName));
        }
        public static ParameterConfigurationForMethod AddPossibleParameter(this AroundMethodInfo aroundInstruction, string parameterName)
        {
            return new ParameterConfigurationForMethod(aroundInstruction, aroundInstruction.Generator.Add(parameterName));
        }

        
    }
}