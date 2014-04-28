using Mono.Cecil;
using NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Engine
{
    public class ParameterConfigurationForInstruction
    {
        private AroundInstructionInfo aroundInstructionInfo;
        private ParameterConfiguration<IlInjectorAvailableVariablesForInstruction> configuration;

        public ParameterConfigurationForInstruction(AroundInstructionInfo aroundInstructionInfo, ParameterConfiguration<IlInjectorAvailableVariablesForInstruction> configuration)
        {
            this.aroundInstructionInfo = aroundInstructionInfo;
            this.configuration = configuration;
        }
    }

    public class ParameterConfigurationForMethod
    {
        private AroundMethodInfo aroundMethodInfo;
        private ParameterConfiguration<IlInjectorAvailableVariables> configuration;

        public ParameterConfigurationForMethod(AroundMethodInfo aroundMethodInfo, ParameterConfiguration<IlInjectorAvailableVariables> configuration)
        {
            this.aroundMethodInfo = aroundMethodInfo;
            this.configuration = configuration;
        }
    }


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