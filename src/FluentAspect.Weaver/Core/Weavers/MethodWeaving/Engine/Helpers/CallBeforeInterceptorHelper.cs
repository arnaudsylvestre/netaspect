using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Model;
using FluentAspect.Weaver.Core.Weavers.Methods;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Helpers
{
    public static class CallBeforeInterceptorHelper
    {
        public static void CallBefore(this MethodToWeave method, Variables variables)
        {
            var caller = new InterceptorCaller(method.Method);
            caller.AddCommonVariables(method.Method.MethodDefinition, variables);
            caller.Call(method, variables, configuration => configuration.Before);
        }
    }
}