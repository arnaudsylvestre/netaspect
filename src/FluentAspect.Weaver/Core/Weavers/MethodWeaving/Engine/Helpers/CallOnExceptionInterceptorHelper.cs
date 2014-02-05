using System;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Model;
using FluentAspect.Weaver.Helpers.IL;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Helpers
{
    public static class CallOnExceptionInterceptorHelper
    {
        public static void GenerateOnExceptionInterceptor(this MethodToWeave myMethod, Variables variables, Collection<Instruction> instructions)
        {
            var exception = myMethod.Method.MethodDefinition.CreateVariable<Exception>();
            CallExceptionInterceptor(myMethod, variables, exception, instructions);
            instructions.AppendThrow();
        }

        private static void CallExceptionInterceptor(MethodToWeave method, Variables variables, VariableDefinition ex, Collection<Instruction> instructions)
        {
            instructions.Add(Instruction.Create(OpCodes.Stloc, ex));
            var caller = new InterceptorCaller(instructions, method.Method.MethodDefinition);
            caller.AddCommonVariables(method.Method.MethodDefinition, variables);
            caller.AddVariable("exception", ex, false);
            caller.Call(method, variables, configuration => configuration.OnException);
        }
    }
}