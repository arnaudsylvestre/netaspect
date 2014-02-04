using System.Collections.Generic;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Model;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Helpers
{
    public static class CallAfterInterceptorHelper
    {
        public static void CallAfter(this MethodToWeave method, Variables variables, Collection<Instruction> instructions)
        {
            var caller = new InterceptorCaller(instructions, method.Method.MethodDefinition);

            caller.AddCommonVariables(method.Method.MethodDefinition, variables);
            caller.AddVariable("result", variables.handleResult, true);
            caller.Call(method, variables, configuration => configuration.After);
        }
    }
}