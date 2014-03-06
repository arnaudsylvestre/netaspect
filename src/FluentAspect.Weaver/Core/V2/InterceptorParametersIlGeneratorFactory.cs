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
        public static void CreateIlGeneratorForInstanceParameter<T>(this ParametersIlGenerator<T> ilGeneratoir,
                                                                    MethodDefinition method)
        {
            ilGeneratoir.Add("instance", new InstanceInterceptorParametersIlGenerator<T>());
        }

        public static void CreateIlGeneratorForCalledParameter<T>(this ParametersIlGenerator<T> ilGeneratoir,
                                                                  MethodDefinition method)
        {
            ilGeneratoir.Add("called", new InstanceInterceptorParametersIlGenerator<T>());
        }

        public static void CreateIlGeneratorForMethodParameter<T>(this ParametersIlGenerator<T> ilGeneratoir)
            where T : IlInstructionInjectorAvailableVariables
        {
            ilGeneratoir.Add("method", new MethodInterceptorParametersIlGenerator<T>());
        }

        public static void CreateIlGeneratorForPropertyParameter(
            this ParametersIlGenerator<IlInjectorAvailableVariables> ilGeneratoir)
        {
            ilGeneratoir.Add("property", new PropertyInterceptorParametersIlGenerator());
        }

        public static void CreateIlGeneratorForResultParameter(
            this ParametersIlGenerator<IlInjectorAvailableVariables> ilGeneratoir)
        {
            ilGeneratoir.Add("result", new ResultInterceptorParametersIlGenerator());
        }

        public static void CreateIlGeneratorForExceptionParameter(
            this ParametersIlGenerator<IlInjectorAvailableVariables> ilGeneratoir)
        {
            ilGeneratoir.Add("exception", new ExceptionInterceptorParametersIlGenerator());
        }

        public static void CreateIlGeneratorForParameterNameParameter<T>(this ParametersIlGenerator<T> ilGeneratoir,
                                                                         MethodDefinition method)
        {
            foreach (ParameterDefinition parameterDefinition in method.Parameters)
            {
                ilGeneratoir.Add(parameterDefinition.Name.ToLower(),
                                 new ParameterNameInterceptorParametersIlGenerator<T>(parameterDefinition));
            }
        }


        public static void CreateIlGeneratorForPropertySetValueParameter<T>(this ParametersIlGenerator<T> ilGeneratoir,
                                                                            MethodDefinition method)
        {
            ilGeneratoir.Add("value", new ParameterNameInterceptorParametersIlGenerator<T>(method.Parameters[0]));
        }

        public static void CreateIlGeneratorForParametersParameter<T>(this ParametersIlGenerator<T> ilGeneratoir,
                                                                      MethodDefinition method)
            where T : IlInstructionInjectorAvailableVariables
        {
            ilGeneratoir.Add("parameters", new ParametersInterceptorParametersIlGenerator<T>());
        }
    }

    public class ExceptionInterceptorParametersIlGenerator :
        IInterceptorParameterIlGenerator<IlInjectorAvailableVariables>
    {
        public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions,
                               IlInjectorAvailableVariables info)
        {
            instructions.Add(Instruction.Create(OpCodes.Ldloc, info.Exception));
        }
    }

    public class ResultInterceptorParametersIlGenerator : IInterceptorParameterIlGenerator<IlInjectorAvailableVariables>
    {
        public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions,
                               IlInjectorAvailableVariables info)
        {
            instructions.Add(Instruction.Create(parameterInfo.ParameterType.IsByRef ? OpCodes.Ldloca : OpCodes.Ldloc,
                                                info.Result));
        }
    }

    public class ParametersInterceptorParametersIlGenerator<T> : IInterceptorParameterIlGenerator<T>
        where T : IlInstructionInjectorAvailableVariables
    {
        public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, T info)
        {
            instructions.Add(Instruction.Create(OpCodes.Ldloc, info.Parameters));
        }
    }
}