using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.Checkers;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Engine
{
    public class ParameterConfigurationForInstruction
    {
        private AroundInstructionInfo aroundInstructionInfo;
        private ParameterConfiguration<IlInjectorAvailableVariablesForInstruction> configuration;
        private List<string> allowedTypes = new List<string>();

        public ParameterConfigurationForInstruction(AroundInstructionInfo aroundInstructionInfo, ParameterConfiguration<IlInjectorAvailableVariablesForInstruction> configuration)
        {
            this.aroundInstructionInfo = aroundInstructionInfo;
            this.configuration = configuration;
        }

        public ParameterConfigurationForInstruction WhichCanNotBeReferenced()
        {
            configuration.Checker.Checkers.Add((parameter, errorListener) => Ensure.NotReferenced(parameter, errorListener));
            return this;
        }

        public ParameterConfigurationForInstruction WhichPdbPresent()
        {
            configuration.Checker.Checkers.Add((parameter, errorListener) => Ensure.SequencePoint(aroundInstructionInfo.Instruction, errorListener, parameter));
            return this;
        }

        public ParameterConfigurationForInstruction WhichCanNotBeOut()
        {
            configuration.Checker.Checkers.Add((parameter, errorListener) => Ensure.NotOut(parameter, errorListener));
            return this;
        }

        public ParameterConfigurationForInstruction WhereFieldCanNotBeStatic()
        {
            configuration.Checker.Checkers.Add((parameter, errorListener) => Ensure.NotStaticButDefaultValue(parameter, errorListener, aroundInstructionInfo.GetOperandAsField()));
            return this;
        }

        public ParameterConfigurationForInstruction WhereCurrentMethodCanNotBeStatic()
        {
            configuration.Checker.Checkers.Add((parameter, errorListener) => Ensure.NotStatic(parameter, errorListener, aroundInstructionInfo.Method));
            return this;
        }

        public ParameterConfigurationForInstruction WhichMustBeOfType<T1>()
        {
            allowedTypes.Add(typeof(T1).FullName);
            return this;
        }

        public ParameterConfigurationForInstruction WhichMustBeOfTypeOfParameter(ParameterDefinition parameterDefinition)
        {
            configuration.Checker.Checkers.Add((info, handler) => Ensure.OfType(info, handler, parameterDefinition));
            return this;
        }

        public ParameterConfigurationForInstruction OrOfType(TypeReference type)
        {
            return WhichMustBeOfTypeOf(type);
        }

        public ParameterConfigurationForInstruction WhichMustBeOfTypeOf(TypeReference type)
        {
            allowedTypes.Add(type.FullName);
            return this;
        }

        public ParameterConfigurationForInstruction OrOfFieldDeclaringType()
        {
            return OrOfType(aroundInstructionInfo.GetOperandAsField().DeclaringType);
        }

        public ParameterConfigurationForInstruction OrOfCurrentMethodDeclaringType()
        {
            return OrOfType(aroundInstructionInfo.Method.DeclaringType);
        }


        public ParameterConfigurationForInstruction AndInjectThePdbInfo(Func<SequencePoint, int> pdbInfoProvider)
        {
            configuration.Generator.Generators.Add((parameter, instructions, info) =>
                {
                    SequencePoint instructionPP = aroundInstructionInfo.Instruction.GetLastSequencePoint();
                    instructions.Add(Instruction.Create(OpCodes.Ldc_I4,
                                                        instructionPP == null
                                                            ? 0
                                                            : pdbInfoProvider(instructionPP)));
                });
            return this;
        }
        public ParameterConfigurationForInstruction AndInjectThePdbInfo(Func<SequencePoint, string> pdbInfoProvider)
        {
            configuration.Generator.Generators.Add((parameter, instructions, info) =>
                {
                    SequencePoint instructionPP = aroundInstructionInfo.Instruction.GetLastSequencePoint();
                    instructions.Add(Instruction.Create(OpCodes.Ldstr,
                                                        instructionPP == null
                                                            ? null
                                                            : pdbInfoProvider(instructionPP)));
                });
            return this;
        }

        public ParameterConfigurationForInstruction AndInjectTheCalledFieldInfo()
        {

            configuration.Generator.Generators.Add((parameter, instructions, info) =>
                {
                    var interceptor = aroundInstructionInfo;
                    instructions.AppendCallToTargetGetType(interceptor.Method.Module, info.Called);
                    instructions.AppendCallToGetField(interceptor.GetOperandAsField().Name, interceptor.Method.Module);
                });
            return this;
        }
        public ParameterConfigurationForInstruction AndInjectTheCalledPropertyInfo()
        {

            configuration.Generator.Generators.Add((parameter, instructions, info) =>
                {
                    var interceptor = aroundInstructionInfo;
                    instructions.AppendCallToTargetGetType(interceptor.Method.Module, info.Called);
                    instructions.AppendCallToGetProperty(interceptor.GetOperandAsMethod().GetProperty().Name, interceptor.Method.Module);
                });
            return this;
        }

        public ParameterConfigurationForInstruction AndInjectTheCalledInstance()
        {
            configuration.Generator.Generators.Add((parameter, instructions, info) =>
                {
                    var called = info.Called;
                    instructions.Add(called == null
                                         ? Instruction.Create(OpCodes.Ldnull)
                                         : Instruction.Create(OpCodes.Ldloc, called));
                });
            return this;
        }
        public ParameterConfigurationForInstruction AndInjectTheCurrentInstance()
        {
            configuration.Generator.Generators.Add((parameter, instructions, info) =>
                {
                    instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
                });
            return this;
        }
        public ParameterConfigurationForInstruction AndInjectTheCurrentMethod()
        {
            configuration.Generator.Generators.Add((parameter, instructions, info) =>
                {
                    instructions.Add(Instruction.Create(OpCodes.Ldloc, info.CurrentMethodBase));
                });
            return this;
        }
        public ParameterConfigurationForInstruction AndInjectTheVariable(Func<IlInjectorAvailableVariablesForInstruction, VariableDefinition> variableProvider)
        {
            configuration.Generator.Generators.Add((parameter, instructions, info) =>
                {
                    instructions.Add(Instruction.Create(OpCodes.Ldloc, variableProvider(info)));
                });
            return this;
        }

        public void AndInjectTheCalledParameter(ParameterDefinition parameter)
        {
            configuration.Generator.Generators.Add((parameterInfo, instructions, info) =>
                {
                    ModuleDefinition moduleDefinition = ((MethodDefinition)parameter.Method).Module;
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
        }


        public ParameterConfigurationForInstruction AndInjectTheParameter(ParameterDefinition parameter)
        {
            configuration.Generator.Generators.Add((parameterInfo, instructions, info) =>
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
            return this;
        }
    }
}