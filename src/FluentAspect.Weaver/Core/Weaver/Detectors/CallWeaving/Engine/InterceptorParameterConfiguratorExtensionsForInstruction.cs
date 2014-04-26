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

        public static InterceptorParameterConfigurator<IlInjectorAvailableVariablesForInstruction> AndInjectTheCalledFieldInfo(this InterceptorParameterConfigurator<IlInjectorAvailableVariablesForInstruction> interceptorParameterConfigurator)
        {

            interceptorParameterConfigurator.Generator.Generators.Add((parameter, instructions, info) =>
            {
                var interceptor = interceptorParameterConfigurator.Interceptor;
                instructions.AppendCallToTargetGetType(interceptor.Method.Module, info.Called);
                instructions.AppendCallToGetField(interceptor.GetOperandAsField().Name, interceptor.Method.Module);
            });
            return interceptorParameterConfigurator;
        }
        public static InterceptorParameterConfigurator<IlInjectorAvailableVariablesForInstruction> AndInjectTheCalledPropertyInfo(this InterceptorParameterConfigurator<IlInjectorAvailableVariablesForInstruction> interceptorParameterConfigurator)
        {

            interceptorParameterConfigurator.Generator.Generators.Add((parameter, instructions, info) =>
            {
                var interceptor = interceptorParameterConfigurator.Interceptor;
                instructions.AppendCallToTargetGetType(interceptor.Method.Module, info.Called);
                instructions.AppendCallToGetProperty(interceptor.GetOperandAsMethod().GetProperty().Name, interceptor.Method.Module);
            });
            return interceptorParameterConfigurator;
        }

        public static InterceptorParameterConfigurator<IlInjectorAvailableVariablesForInstruction> AndInjectTheCalledInstance(this InterceptorParameterConfigurator<IlInjectorAvailableVariablesForInstruction> interceptorParameterConfigurator)
        {
            interceptorParameterConfigurator.Generator.Generators.Add((parameter, instructions, info) =>
                {
                    var called = info.Called;
                    instructions.Add(called == null
                                         ? Instruction.Create(OpCodes.Ldnull)
                                         : Instruction.Create(OpCodes.Ldloc, called));
                });
            return interceptorParameterConfigurator;
        }
        public static InterceptorParameterConfigurator<IlInjectorAvailableVariablesForInstruction> AndInjectTheCurrentInstance(this InterceptorParameterConfigurator<IlInjectorAvailableVariablesForInstruction> interceptorParameterConfigurator)
        {
            interceptorParameterConfigurator.Generator.Generators.Add((parameter, instructions, info) =>
                {
                    instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
                });
            return interceptorParameterConfigurator;
        }
        public static InterceptorParameterConfigurator<IlInjectorAvailableVariablesForInstruction> AndInjectTheCurrentMethod(this InterceptorParameterConfigurator<IlInjectorAvailableVariablesForInstruction> interceptorParameterConfigurator)
        {
            interceptorParameterConfigurator.Generator.Generators.Add((parameter, instructions, info) =>
                {
                    instructions.Add(Instruction.Create(OpCodes.Ldloc, info.CurrentMethodBase));
                });
            return interceptorParameterConfigurator;
        }
        public static InterceptorParameterConfigurator<IlInjectorAvailableVariablesForInstruction> AndInjectTheVariable(this InterceptorParameterConfigurator<IlInjectorAvailableVariablesForInstruction> interceptorParameterConfigurator, Func<IlInjectorAvailableVariablesForInstruction, VariableDefinition> variableProvider)
        {
            interceptorParameterConfigurator.Generator.Generators.Add((parameter, instructions, info) =>
                {
                    instructions.Add(Instruction.Create(OpCodes.Ldloc, variableProvider(info)));
                });
            return interceptorParameterConfigurator;
        }

        public static void AndInjectTheCalledParameter(this InterceptorParameterConfigurator<IlInjectorAvailableVariablesForInstruction> interceptorParameterConfigurator, ParameterDefinition parameter)
        {
            interceptorParameterConfigurator.Generator.Generators.Add((parameterInfo, instructions, info) =>
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


        public static InterceptorParameterConfigurator<IlInjectorAvailableVariablesForInstruction> AndInjectTheParameter(this InterceptorParameterConfigurator<IlInjectorAvailableVariablesForInstruction> interceptorParameterConfigurator, ParameterDefinition parameter)
        {
            interceptorParameterConfigurator.Generator.Generators.Add((parameterInfo, instructions, info) =>
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
            return interceptorParameterConfigurator;
        }
    }
}