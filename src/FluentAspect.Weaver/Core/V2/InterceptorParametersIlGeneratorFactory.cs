using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.V2;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters
{
    public static class InterceptorParametersIlGeneratorFactory
    {
        public static void CreateIlGeneratorForInstanceParameter(this ParametersIlGenerator ilGeneratoir, MethodDefinition method)
        {
            ilGeneratoir.Add("instance", new InstanceInterceptorParametersIlGenerator());
        }
        public static void CreateIlGeneratorForMethodParameter(this ParametersIlGenerator ilGeneratoir)
        {
            ilGeneratoir.Add("method", new MethodInterceptorParametersIlGenerator());
        }
        public static void CreateIlGeneratorForPropertyParameter(this ParametersIlGenerator ilGeneratoir)
        {
            ilGeneratoir.Add("property", new PropertyInterceptorParametersIlGenerator());
        }
        public static void CreateIlGeneratorForResultParameter(this ParametersIlGenerator ilGeneratoir)
        {
            ilGeneratoir.Add("result", new ResultInterceptorParametersIlGenerator());
        }
        public static void CreateIlGeneratorForExceptionParameter(this ParametersIlGenerator ilGeneratoir)
        {
            ilGeneratoir.Add("exception", new ExceptionInterceptorParametersIlGenerator());
        }
        public static void CreateIlGeneratorForParameterNameParameter(this ParametersIlGenerator ilGeneratoir, MethodDefinition method)
        {
            foreach (var parameterDefinition in method.Parameters)
            {
                ilGeneratoir.Add(parameterDefinition.Name.ToLower(), new ParameterNameInterceptorParametersIlGenerator(parameterDefinition));

            }

        }


        public static void CreateIlGeneratorForPropertySetValueParameter(this ParametersIlGenerator ilGeneratoir, MethodDefinition method)
        {
            ilGeneratoir.Add("value", new ParameterNameInterceptorParametersIlGenerator(method.Parameters[0]));
        }

        public static void CreateIlGeneratorForParametersParameter(this ParametersIlGenerator ilGeneratoir, MethodDefinition method)
        {
            ilGeneratoir.Add("parameters", new ParametersInterceptorParametersIlGenerator());

        }

        //engine.AddPossibleParameter(parameter1.Name,
        //                                (p, handler) =>
        //                                {
        //                                    Ensure.OfType(p, handler, parameter1);
        //                                }, (p, _instructions) =>
        //                                    {
        //                                        var moduleDefinition = ((MethodDefinition)parameter.Method).Module;
        //                                        if (p.ParameterType.IsByRef && !parameter.ParameterType.IsByReference)
        //                                        {
        //                                            _instructions.Add(Instruction.Create(OpCodes.Ldarga, parameter));

        //                                        }
        //                                        else if (!p.ParameterType.IsByRef && parameter.ParameterType.IsByReference)
        //                                        {
        //                                            _instructions.Add(Instruction.Create(OpCodes.Ldarg, parameter));
        //                                            _instructions.Add(Instruction.Create(OpCodes.Ldobj, moduleDefinition.Import(p.ParameterType)));
        //                                        }
        //                                        else
        //                                        {
        //                                            _instructions.Add(Instruction.Create(OpCodes.Ldarg, parameter));

        //                                        }
        //                                        if (parameter.ParameterType != moduleDefinition.TypeSystem.Object &&
        //                                            p.ParameterType == typeof(Object))
        //                                        {
        //                                            TypeReference reference = parameter.ParameterType;
        //                                            if (reference.IsByReference)
        //                                            {
        //                                                reference = ((MethodDefinition)parameter.Method).GenericParameters.First(t => t.Name == reference.Name.TrimEnd('&'));
        //                                                _instructions.Add(Instruction.Create(OpCodes.Ldobj, reference));

        //                                            }
        //                                            _instructions.Add(Instruction.Create(OpCodes.Box, reference));
        //                                        }
        //                                    }
        //                                    );
    }

    public class ExceptionInterceptorParametersIlGenerator : IInterceptorParameterIlGenerator
    {
        public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, IlInjectorAvailableVariables info)
        {
            instructions.Add(Instruction.Create(OpCodes.Ldloc, info.Exception));
        }
    }

    public class ResultInterceptorParametersIlGenerator : IInterceptorParameterIlGenerator
    {
        public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, IlInjectorAvailableVariables info)
        {
            instructions.Add(Instruction.Create(parameterInfo.ParameterType.IsByRef ? OpCodes.Ldloca : OpCodes.Ldloc,
                                                info.Result));
        }
    }

    public class ParametersInterceptorParametersIlGenerator : IInterceptorParameterIlGenerator
    {
        public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, IlInjectorAvailableVariables info)
        {
            instructions.Add(Instruction.Create(OpCodes.Ldloc, info.Parameters));
        }
    }
}