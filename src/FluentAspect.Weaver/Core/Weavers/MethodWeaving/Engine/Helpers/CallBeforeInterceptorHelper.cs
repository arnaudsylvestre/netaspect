using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Model;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Helpers
{
    public static class CallBeforeInterceptorHelper
    {
        public static void CallBefore(this MethodToWeave method, Variables variables, Collection<Instruction> beforeInstructions)
        {
            var caller = new InterceptorCaller(beforeInstructions, method.Method.MethodDefinition);
            caller.AddCommonVariables(method.Method.MethodDefinition, variables);
            caller.Call(method, variables, configuration => configuration.Before);
        }
    }
}