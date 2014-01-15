using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.Model.Helpers;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine.Model;
using FluentAspect.Weaver.Helpers;
using FluentAspect.Weaver.Helpers.IL;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine
{

    public class CallFieldWeaver : IWeaveable
    {
        private CallToWeave toWeave;


        public CallFieldWeaver(MethodPoint point,
                                IEnumerable<CallWeavingConfiguration> interceptorTypes)
        {
            toWeave = new CallToWeave
                {
                    MethodToWeave = point.Method,
                    Instruction                    = point.Instruction,
                    Interceptors                    = interceptorTypes
                };
        }

        public void Weave(ErrorHandler errorP_P)
        {
            var reference = toWeave.Instruction.Operand as FieldReference;
            toWeave.MethodToWeave.Body.InitLocals = true;
            SequencePoint point_L = toWeave.Instruction.GetLastSequencePoint();

            List<Instruction> instructions = new List<Instruction>();
            
           instructions.AddRange(CreateBeforeInstructions(toWeave.MethodToWeave.Module, point_L, reference));

            var afterInstructions = CreateAfterInstructions(toWeave.MethodToWeave.Module, point_L, reference).ToList();
            toWeave.MethodToWeave.InsertAfter(toWeave.Instruction, afterInstructions);
            toWeave.MethodToWeave.InsertBefore(toWeave.Instruction, instructions);

        }

        public void Check(ErrorHandler errorHandler)
        {
            foreach (var netAspectAttribute in toWeave.Interceptors)
            {
                CheckParameters(netAspectAttribute.BeforeInterceptor.GetParameters(), errorHandler);
                CheckParameters(netAspectAttribute.AfterInterceptor.GetParameters(), errorHandler);
            }
        }

        private void CheckParameters(IEnumerable<ParameterInfo> getParameters, ErrorHandler errorHandler)
        {
            var errors = new Dictionary<string, Action<ParameterInfo, ErrorHandler>>();
            errors.Add("linenumber", (info, handler) =>
            {
                EnsureSequencePoint(errorHandler, info);
                EnsureType<int>(info, errorHandler);
            });
            errors.Add("columnnumber", (info, handler) =>
            {
                EnsureSequencePoint(errorHandler, info);
                EnsureType<int>(info, errorHandler);
            });
            errors.Add("filename", (info, handler) =>
            {
                EnsureSequencePoint(errorHandler, info);
                EnsureType<string>(info, errorHandler);
            });
            errors.Add("filepath", (info, handler) =>
            {
                EnsureSequencePoint(errorHandler, info);
                EnsureType<string>(info, errorHandler);
            });
            errors.Add("caller", (info, handler) =>
            {
                EnsureSequencePoint(errorHandler, info);
                EnsureType(info, errorHandler, toWeave.MethodToWeave.DeclaringType, typeof(object));
            });

            foreach (ParameterDefinition parameter_L in toWeave.MethodToWeave.Parameters)
            {
                ParameterDefinition parameter1_L = parameter_L;
                errors.Add((parameter1_L.Name + "Caller").ToLower(), (info, handler) =>
                {
                    EnsureType(info, errorHandler, parameter1_L.ParameterType, null);
                });
            }


            foreach (var parameterInfo in getParameters)
            {
                errors[parameterInfo.Name.ToLower()](parameterInfo, errorHandler);
            }
        }

        private void EnsureType(ParameterInfo info, ErrorHandler errorHandler, TypeReference declaringType, Type type)
        {
            var secondTypeOk = true;
            if (type != null)
                secondTypeOk = info.ParameterType == type;
            if (info.ParameterType.FullName != declaringType.FullName && !secondTypeOk)
                errorHandler.Errors.Add(string.Format("Wrong parameter type for {0} in method {1} of type {2}", info.Name, info.Member.Name, info.Member.DeclaringType.FullName));
        }

        private void EnsureType<T>(ParameterInfo info, ErrorHandler errorHandler)
        {
            if (info.ParameterType != typeof(T))
                errorHandler.Errors.Add(string.Format("Wrong parameter type for {0} in method {1} of type {2}", info.Name, info.Member.Name, info.Member.DeclaringType.FullName));
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

        private IEnumerable<Instruction> CreateAfterInstructions(ModuleDefinition module, SequencePoint instructionP_P, FieldReference reference)
        {
            var instructions = new List<Instruction>();
            foreach (var interceptorType in toWeave.Interceptors)
            {
                MethodInfo afterCallMethod = interceptorType.AfterFieldAccess.Method;
                if (afterCallMethod != null)
                {
                    var parameters = new Dictionary<string, Action<ParameterInfo>>();
                    FillParameters(instructionP_P, parameters, instructions, reference);


                    foreach (ParameterInfo parameterInfo_L in afterCallMethod.GetParameters())
                    {
                        parameters[parameterInfo_L.Name.ToLower()](parameterInfo_L);
                    }

                    instructions.Add(Instruction.Create(OpCodes.Call, module.Import(afterCallMethod)));
                }
            }
            return instructions;
        }

        private void FillParameters(SequencePoint instructionP_P, Dictionary<string, Action<ParameterInfo>> parameters, List<Instruction> instructions, FieldReference reference_P)
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

        private IEnumerable<Instruction> CreateBeforeInstructions(ModuleDefinition module, SequencePoint pointL, FieldReference reference)
        {
            var instructions = new List<Instruction>();
            foreach (var interceptorType in toWeave.Interceptors)
            {
                if (interceptorType.BeforeFieldAccess.Method != null)
                {
                    var parameters = new Dictionary<string, Action<ParameterInfo>>();
                    FillParameters(pointL, parameters, instructions, reference);
                    instructions.Add(Instruction.Create(OpCodes.Call,
                                                        module.Import(
                                                            interceptorType.BeforeFieldAccess
                                                                           .Method)));
                }
            }
            return instructions;
        }
    }
}