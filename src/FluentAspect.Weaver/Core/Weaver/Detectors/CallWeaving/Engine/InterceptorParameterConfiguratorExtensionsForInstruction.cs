using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Engine
{
    public static class InterceptorParameterConfiguratorExtensionsForInstruction
    {

        public static AroundInstructionParametersConfigurator<IlInjectorAvailableVariablesForInstruction, AroundInstructionInfo> AndInjectTheCalledFieldInfo(this AroundInstructionParametersConfigurator<IlInjectorAvailableVariablesForInstruction, AroundInstructionInfo> aroundInstructionParametersConfigurator)
        {

            aroundInstructionParametersConfigurator.Generator.Generators.Add((parameter, instructions, info) =>
            {
                var interceptor = aroundInstructionParametersConfigurator.AroundInstruction;
                instructions.AppendCallToTargetGetType(interceptor.Method.Module, info.Called);
                instructions.AppendCallToGetField(interceptor.GetOperandAsField().Name, interceptor.Method.Module);
            });
            return aroundInstructionParametersConfigurator;
        }
        public static AroundInstructionParametersConfigurator<IlInjectorAvailableVariablesForInstruction, AroundInstructionInfo> AndInjectTheCalledPropertyInfo(this AroundInstructionParametersConfigurator<IlInjectorAvailableVariablesForInstruction, AroundInstructionInfo> aroundInstructionParametersConfigurator)
        {

            aroundInstructionParametersConfigurator.Generator.Generators.Add((parameter, instructions, info) =>
            {
                var interceptor = aroundInstructionParametersConfigurator.AroundInstruction;
                instructions.AppendCallToTargetGetType(interceptor.Method.Module, info.Called);
                instructions.AppendCallToGetProperty(interceptor.GetOperandAsMethod().GetProperty().Name, interceptor.Method.Module);
            });
            return aroundInstructionParametersConfigurator;
        }

        public static AroundInstructionParametersConfigurator<IlInjectorAvailableVariablesForInstruction, AroundInstructionInfo> AndInjectTheCalledInstance(this AroundInstructionParametersConfigurator<IlInjectorAvailableVariablesForInstruction, AroundInstructionInfo> aroundInstructionParametersConfigurator)
        {
            aroundInstructionParametersConfigurator.Generator.Generators.Add((parameter, instructions, info) =>
                {
                    var called = info.Called;
                    instructions.Add(called == null
                                         ? Instruction.Create(OpCodes.Ldnull)
                                         : Instruction.Create(OpCodes.Ldloc, called));
                });
            return aroundInstructionParametersConfigurator;
        }
        public static AroundInstructionParametersConfigurator<IlInjectorAvailableVariablesForInstruction, AroundInstructionInfo> AndInjectTheCurrentInstance(this AroundInstructionParametersConfigurator<IlInjectorAvailableVariablesForInstruction, AroundInstructionInfo> aroundInstructionParametersConfigurator)
        {
            aroundInstructionParametersConfigurator.Generator.Generators.Add((parameter, instructions, info) =>
                {
                    instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
                });
            return aroundInstructionParametersConfigurator;
        }
        public static AroundInstructionParametersConfigurator<IlInjectorAvailableVariablesForInstruction, AroundInstructionInfo> AndInjectTheCurrentMethod(this AroundInstructionParametersConfigurator<IlInjectorAvailableVariablesForInstruction, AroundInstructionInfo> aroundInstructionParametersConfigurator)
        {
            aroundInstructionParametersConfigurator.Generator.Generators.Add((parameter, instructions, info) =>
                {
                    instructions.Add(Instruction.Create(OpCodes.Ldloc, info.CurrentMethodBase));
                });
            return aroundInstructionParametersConfigurator;
        }
        public static AroundInstructionParametersConfigurator<IlInjectorAvailableVariablesForInstruction, AroundInstructionInfo> AndInjectTheVariable(this AroundInstructionParametersConfigurator<IlInjectorAvailableVariablesForInstruction, AroundInstructionInfo> aroundInstructionParametersConfigurator, Func<IlInjectorAvailableVariablesForInstruction, VariableDefinition> variableProvider)
        {
            aroundInstructionParametersConfigurator.Generator.Generators.Add((parameter, instructions, info) =>
                {
                    instructions.Add(Instruction.Create(OpCodes.Ldloc, variableProvider(info)));
                });
            return aroundInstructionParametersConfigurator;
        }

        public static void AndInjectTheCalledParameter(this AroundInstructionParametersConfigurator<IlInjectorAvailableVariablesForInstruction, AroundInstructionInfo> aroundInstructionParametersConfigurator, ParameterDefinition parameter)
        {
            aroundInstructionParametersConfigurator.Generator.Generators.Add((parameterInfo, instructions, info) =>
                {
                    ModuleDefinition moduleDefinition = ((MethodDefinition) parameter.Method).Module;
                    if (!parameterInfo.ParameterType.IsByRef && parameter.ParameterType.IsByReference)
                    {
                        instructions.Add(Instruction.Create(OpCodes.Ldloc,
                                                            info.CalledParameters["called" + parameter.Name]));
                        instructions.Add(Instruction.Create(OpCodes.Ldobj,
                                                            moduleDefinition.Import(parameterInfo.ParameterType)));
                    }
                    else
                    {
                        instructions.Add(Instruction.Create(OpCodes.Ldloc,
                                                            info.CalledParameters["called" + parameter.Name]));
                    }
                    if (parameter.ParameterType != moduleDefinition.TypeSystem.Object &&
                        parameterInfo.ParameterType == typeof (Object))
                    {
                        TypeReference reference = parameter.ParameterType;
                        if (reference.IsByReference)
                        {
                            reference =
                                ((MethodDefinition) parameter.Method).GenericParameters.First(
                                    t => t.Name == reference.Name.TrimEnd('&'));
                            instructions.Add(Instruction.Create(OpCodes.Ldobj, reference));
                        }
                        instructions.Add(Instruction.Create(OpCodes.Box, reference));
                    }
                });
        }


        public static AroundInstructionParametersConfigurator<IlInjectorAvailableVariablesForInstruction, AroundInstructionInfo> AndInjectTheParameter(this AroundInstructionParametersConfigurator<IlInjectorAvailableVariablesForInstruction, AroundInstructionInfo> aroundInstructionParametersConfigurator, ParameterDefinition parameter)
        {
            aroundInstructionParametersConfigurator.Generator.Generators.Add((parameterInfo, instructions, info) =>
                {
                    ModuleDefinition moduleDefinition = ((MethodDefinition)parameter.Method).Module;
                    if (parameterInfo.ParameterType.IsByRef && !parameter.ParameterType.IsByReference)
                    {
                        instructions.Add(Instruction.Create(OpCodes.Ldarga, parameter));
                    }
                    else if (!parameterInfo.ParameterType.IsByRef && parameter.ParameterType.IsByReference)
                    {
                        instructions.Add(Instruction.Create(OpCodes.Ldarg, parameter));
                        instructions.Add(Instruction.Create(OpCodes.Ldobj, moduleDefinition.Import(parameterInfo.ParameterType)));
                    }
                    else
                    {
                        instructions.Add(Instruction.Create(OpCodes.Ldarg, parameter));
                    }
                    if (parameter.ParameterType != moduleDefinition.TypeSystem.Object &&
                        parameterInfo.ParameterType == typeof(Object))
                    {
                        TypeReference reference = parameter.ParameterType;
                        if (reference.IsByReference)
                        {
                            reference =
                                ((MethodDefinition)parameter.Method).GenericParameters.First(
                                    t => t.Name == reference.Name.TrimEnd('&'));
                            instructions.Add(Instruction.Create(OpCodes.Ldobj, reference));
                        }
                        instructions.Add(Instruction.Create(OpCodes.Box, reference));
                    }
                });
            return aroundInstructionParametersConfigurator;
        }
    }
}