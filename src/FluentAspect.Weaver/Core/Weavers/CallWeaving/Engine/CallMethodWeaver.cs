using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.Weavers.Helpers;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace FluentAspect.Weaver.Core.Weavers.Calls
{
    public class CallMethodWeaver : IWeaveable
    {
        private CallToWeave toWeave;


        public CallMethodWeaver(MethodDefinition method, Instruction instruction,
                                List<CallWeavingConfiguration> interceptorTypes)
        {
            toWeave = new CallToWeave
                {
                    MethodToWeave = method,
                    Instruction                    = instruction,
                    Interceptors                    = interceptorTypes
                };
        }

        public void Weave(ErrorHandler errorP_P)
        {
            var reference = toWeave.Instruction.Operand as MethodReference;

            SequencePoint point_L = toWeave.Instruction.GetLastSequencePoint();

           toWeave.MethodToWeave.InsertAfter(toWeave.Instruction, CreateAfterInstructions(toWeave.MethodToWeave.Module, point_L));
           toWeave.MethodToWeave.InsertBefore(toWeave.Instruction, CreateBeforeInstructions(toWeave.MethodToWeave.Module));

            foreach (ParameterDefinition parameter in reference.Parameters)
            {
                var variableDefinition = new VariableDefinition(parameter.ParameterType);
                toWeave.MethodToWeave.Body.Variables.Add(variableDefinition);
            }
        }

        public void Check(ErrorHandler errorHandler)
        {
            foreach (var netAspectAttribute in toWeave.Interceptors)
            {
                //CheckParameters(netAspectAttribute)
            }
        }

        public bool CanWeave()
        {
            return true;
        }

        private IEnumerable<Instruction> CreateAfterInstructions(ModuleDefinition module, SequencePoint instructionP_P)
        {
            var instructions = new List<Instruction>();
            foreach (var interceptorType in toWeave.Interceptors)
            {
                MethodInfo afterCallMethod = interceptorType.AfterInterceptor.Method;
                if (afterCallMethod != null)
                {
                    var parameters = new Dictionary<string, Action<ParameterInfo>>();
                    parameters.Add("linenumber",
                                   (p) =>
                                   instructions.Add(Instruction.Create(OpCodes.Ldc_I4,
                                                                       instructionP_P == null
                                                                           ? 0
                                                                           : instructionP_P.StartLine)));
                    parameters.Add("columnnumber",
                                   (p) =>
                                   instructions.Add(Instruction.Create(OpCodes.Ldc_I4,
                                                                       instructionP_P == null
                                                                           ? 0
                                                                           : instructionP_P.StartColumn)));
                    parameters.Add("filename",
                                   (p) =>
                                   instructions.Add(Instruction.Create(OpCodes.Ldstr,
                                                                       instructionP_P == null
                                                                           ? ""
                                                                           : Path.GetFileName(
                                                                               instructionP_P.Document.Url))));
                    parameters.Add("filepath",
                                   (p) =>
                                   instructions.Add(Instruction.Create(OpCodes.Ldstr,
                                                                       instructionP_P == null
                                                                           ? ""
                                                                           : instructionP_P.Document.Url)));
                    parameters.Add("caller", (p) => instructions.Add(Instruction.Create(OpCodes.Ldarg_0)));

                    foreach (ParameterDefinition parameter_L in toWeave.MethodToWeave.Parameters)
                    {
                        ParameterDefinition parameter1_L = parameter_L;
                        parameters.Add((parameter1_L.Name + "Caller").ToLower(),
                                       (p) => instructions.Add(Instruction.Create(OpCodes.Ldarg, parameter1_L)));
                    }


                    foreach (ParameterInfo parameterInfo_L in afterCallMethod.GetParameters())
                    {
                        parameters[parameterInfo_L.Name.ToLower()](parameterInfo_L);
                    }

                    instructions.Add(Instruction.Create(OpCodes.Call, module.Import(afterCallMethod)));
                }
            }
            return instructions;
        }

        private void EnsureSequencePoint(SequencePoint sequencePoint, ParameterDefinition parameterDefinition, ErrorHandler errorHandler)
        {
            if (sequencePoint == null)
            {
                errorHandler.Warnings.Add(string.Format("The parameter {0} in method {1} of type {2} will have the default value because there is no debugging information",
                    parameterDefinition.Name, ((IMemberDefinition)parameterDefinition.Method).Name, ((IMemberDefinition)parameterDefinition.Method).FullName));
            }
        }

        private IEnumerable<Instruction> CreateBeforeInstructions(ModuleDefinition module)
        {
            var instructions = new List<Instruction>();
            foreach (var interceptorType in toWeave.Interceptors)
            {
                if (interceptorType.BeforeInterceptor.Method != null)
                {
                    instructions.Add(Instruction.Create(OpCodes.Call,
                                                        module.Import(
                                                            interceptorType.BeforeInterceptor
                                                                           .Method)));
                }
            }
            return instructions;
        }
    }
}