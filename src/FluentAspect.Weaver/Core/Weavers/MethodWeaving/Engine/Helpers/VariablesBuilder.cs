using System.Collections.Generic;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Model;
using Mono.Cecil.Cil;

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
                    handleResult = CreateResult(method),
                    methodInfo = CreateMethodInfo(method),
                };
        }

        private static VariableDefinition CreateResult(this MethodToWeave myMethod)
        {
            return myMethod.Method.CreateWeavedResult();
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
            return myMethod.Method.CreateAndInitializeVariable(myMethod.Interceptors);
        }
    }
}