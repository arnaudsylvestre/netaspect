using System.Collections.Generic;
using System.Linq;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Model;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Helpers
{
    public static class VariablesBuilder
    {
        public static Variables CreateVariables(this MethodToWeave method, Collection<VariableDefinition> instructionsP_P, Collection<Instruction> instructions)
        {
            return new Variables
                {
                    Interceptors = CreateInterceptorInstances(method, instructions),
                    args = CreateArgs(method, instructions),
                    handleResult = CreateResult(method),
                    methodInfo = CreateMethodInfo(method, instructions),
                };
        }

        private static VariableDefinition CreateResult(this MethodToWeave myMethod)
        {
            return myMethod.Method.CreateWeavedResult();
        }

        private static VariableDefinition CreateMethodInfo(this MethodToWeave method, Collection<Instruction> instructions)
        {
            VariableDefinition methodInfo = null;

            if (method.Needs(Variables.Method))
               methodInfo = method.Method.CreateMethodInfo(instructions);
            return methodInfo;
        }

        private static VariableDefinition CreateArgs(this MethodToWeave myMethod, Collection<Instruction> instructions)
        {
            VariableDefinition args = null;
            if (myMethod.Needs(Variables.ParameterParameters))
               args = myMethod.Method.CreateArgsArrayFromParameters(instructions);
            return args;
        }

        private static List<VariableDefinition> CreateInterceptorInstances(MethodToWeave myMethod, Collection<Instruction> instructions)
        {
           return myMethod.Method.CreateAndInitializeVariable(instructions, from i in myMethod.Interceptors select i.Type);
        }
    }
}