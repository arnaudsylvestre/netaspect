using System;
using System.Collections.Generic;
using System.Linq;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Model;
using FluentAspect.Weaver.Helpers;
using FluentAspect.Weaver.Helpers.IL;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Helpers
{
    public static class VariablesBuilder
    {
        public static Variables CreateVariables(this MethodToWeave method)
        {
            return new Variables
            {
                Interceptors = CreateInterceptorInstances(method),
                args = CreateArgs(method),
                methodInfo = CreateMethodInfo(method),
                exception = CreateException(method)
            };
        }

        private static VariableDefinition CreateException(MethodToWeave methodToWeave)
        {
           return methodToWeave.Needs(Variables.Exception) ? methodToWeave.Method.MethodDefinition.CreateVariable(typeof(Exception)) : null;
        }

       public static void InitializeVariables(this MethodToWeave method, Variables variables, Collection<Instruction> instructions)
        {
            method.Method.FIllMethod(instructions, variables.methodInfo);
           FillInterceptorInstances(method, instructions, variables.Interceptors);
           method.Method.FillArgsArrayFromParameters(instructions, variables.args);
        }

        private static VariableDefinition CreateMethodInfo(this MethodToWeave method)
        {
            VariableDefinition methodInfo = null;

            if (method.Needs(Variables.Method))
               methodInfo = method.Method.CreateMethodInfo();
            return methodInfo;
        }

        private static VariableDefinition CreateArgs(this MethodToWeave myMethod)
        {
            VariableDefinition args = null;
            if (myMethod.Needs(Variables.ParameterParameters))
               args = myMethod.Method.CreateArgsArrayFromParameters();
            return args;
        }

        private static List<VariableDefinition> CreateInterceptorInstances(MethodToWeave myMethod)
        {
            return myMethod.Method.CreateVariable(from i in myMethod.Interceptors select i.Type);
        }

        private static void FillInterceptorInstances(MethodToWeave myMethod, Collection<Instruction> instructions, List<VariableDefinition> variables)
        {
            for (int i = 0; i < variables.Count; i++)
            {
                instructions.InitializeInterceptors(myMethod.Method.MethodDefinition, myMethod.Interceptors[i].Type, variables[i]);
                
            }
        }
    }
}