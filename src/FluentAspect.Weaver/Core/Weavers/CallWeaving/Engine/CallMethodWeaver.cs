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
           toWeave.MethodToWeave.InsertBefore(toWeave.Instruction, CreateBeforeInstructions(toWeave.MethodToWeave.Module, point_L));

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
                CheckParameters(netAspectAttribute.BeforeInterceptor.GetParameters(), errorHandler);
            }
        }

        private void CheckParameters(IEnumerable<ParameterInfo> getParameters, ErrorHandler errorHandler)
        {
            var errors = new Dictionary<string, Action<ParameterInfo, ErrorHandler>>();
            errors.Add("linenumber", (info, handler) =>
            {
                EnsureSequencePoint(errorHandler, info);
            });
            errors.Add("columnnumber", (info, handler) =>
            {
                EnsureSequencePoint(errorHandler, info);
            });
            errors.Add("filename", (info, handler) =>
            {
                EnsureSequencePoint(errorHandler, info);
            });
            errors.Add("filepath", (info, handler) =>
            {
                EnsureSequencePoint(errorHandler, info);
            });

            foreach (var parameterInfo in getParameters)
            {
                errors[parameterInfo.Name.ToLower()](parameterInfo, errorHandler);
            }
        }

        private void EnsureSequencePoint(ErrorHandler errorHandler, ParameterInfo info)
        {
            if (toWeave.Instruction.GetLastSequencePoint() == null)
                errorHandler.Warnings.Add(string.Format("The parameter {0} in method {1} of type {2} will have the default value because there is no debugging information",
                    info.Name, (info.Member).Name, (info.Member.DeclaringType).FullName));
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
                    FillParameters(instructionP_P, parameters, instructions);


                    foreach (ParameterInfo parameterInfo_L in afterCallMethod.GetParameters())
                    {
                        parameters[parameterInfo_L.Name.ToLower()](parameterInfo_L);
                    }

                    instructions.Add(Instruction.Create(OpCodes.Call, module.Import(afterCallMethod)));
                }
            }
            return instructions;
        }

        private void FillParameters(SequencePoint instructionP_P, Dictionary<string, Action<ParameterInfo>> parameters, List<Instruction> instructions)
        {
            parameters.Add("linenumber", p => instructions.Add(Create(instructionP_P, i => i.StartLine)));
            parameters.Add("columnnumber", p => instructions.Add(Create(instructionP_P, i => i.StartColumn)));
            parameters.Add("filename", p => instructions.Add(Create(instructionP_P, i => Path.GetFileName(i.Document.Url))));
            parameters.Add("filepath", p => instructions.Add(Create(instructionP_P, i => i.Document.Url)));
            parameters.Add("caller", p => instructions.Add(Instruction.Create(OpCodes.Ldarg_0)));

            foreach (ParameterDefinition parameter_L in toWeave.MethodToWeave.Parameters)
            {
                ParameterDefinition parameter1_L = parameter_L;
                parameters.Add((parameter1_L.Name + "Caller").ToLower(), p => instructions.Add(Instruction.Create(OpCodes.Ldarg, parameter1_L)));
            }
        }

        private static Instruction Create(SequencePoint instructionP_P, Func<SequencePoint, int> provider)
        {
            return Instruction.Create(OpCodes.Ldc_I4,
                                      instructionP_P == null
                                          ? 0
                                          : provider(instructionP_P));
        }

        private static Instruction Create(SequencePoint instructionP_P, Func<SequencePoint, string> provider)
        {
            return Instruction.Create(OpCodes.Ldstr,
                                      instructionP_P == null
                                          ? null
                                          : provider(instructionP_P));
        }

        private IEnumerable<Instruction> CreateBeforeInstructions(ModuleDefinition module, SequencePoint pointL)
        {
            var instructions = new List<Instruction>();
            foreach (var interceptorType in toWeave.Interceptors)
            {
                if (interceptorType.BeforeInterceptor.Method != null)
                {
                    var parameters = new Dictionary<string, Action<ParameterInfo>>();
                    FillParameters(pointL, parameters, instructions);
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