using System.Collections.Generic;
using FluentAspect.Weaver.Core.Weavers.Helpers;
using FluentAspect.Weaver.Core.Weavers.Methods.Interceptors;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.Methods
{
    public class VariablesBuilder
    {
        public static Variables CreateVariables(Method myMethod, IEnumerable<MethodWeavingConfiguration> interceptorType)
        {
            return new Variables()
            {
                Interceptors = CreateInterceptorInstances(myMethod, interceptorType),
                args = CreateArgs(myMethod, interceptorType),
                handleResult = CreateResult(myMethod),
                methodInfo = CreateMethodInfo(myMethod, interceptorType),
            };
        }

        private static VariableDefinition CreateResult(Method myMethod)
        {
            return myMethod.CreateWeavedResult();
        }

        private static VariableDefinition CreateMethodInfo(Method myMethod, IEnumerable<MethodWeavingConfiguration> interceptorType)
        {
            VariableDefinition methodInfo = null;

            if (AroundWeaverConfigurationExtensions.Needs(interceptorType, Variables.Method))
                methodInfo = myMethod.CreateMethodInfo();
            return methodInfo;
        }

        private static VariableDefinition CreateArgs(Method myMethod, IEnumerable<MethodWeavingConfiguration> interceptorType)
        {
            VariableDefinition args = null;
            if (AroundWeaverConfigurationExtensions.Needs(interceptorType, Variables.ParameterParameters))
                args = myMethod.CreateArgsArrayFromParameters();
            return args;
        }

        private static List<VariableDefinition> CreateInterceptorInstances(Method myMethod, IEnumerable<MethodWeavingConfiguration> interceptorType)
        {
            return myMethod.CreateAndInitializeVariable(interceptorType);
        }
    }
}