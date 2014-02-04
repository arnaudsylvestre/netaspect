using System;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Model;
using FluentAspect.Weaver.Helpers.IL;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Helpers
{
    public static class CallOnExceptionInterceptorHelper
    {
        public static void GenerateOnExceptionInterceptor(this MethodToWeave myMethod, Variables variables)
        {
            var exception = myMethod.Method.MethodDefinition.CreateVariable<Exception>();
            CallExceptionInterceptor(myMethod, variables, exception);
            myMethod.Method.MethodDefinition.Body.Instructions.AppendThrow();
        }

        private static void CallExceptionInterceptor(MethodToWeave method, Variables variables, VariableDefinition ex)
        {
             method.Method.MethodDefinition.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc, ex));
            var caller = new InterceptorCaller(method.Method);
            caller.AddCommonVariables(method.Method.MethodDefinition, variables);
            caller.AddVariable("exception", ex, false);
            caller.Call(method, variables, configuration => configuration.OnException);
        }
    }
}