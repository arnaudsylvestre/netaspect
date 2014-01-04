using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Model;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Helpers
{
    public static class CallAfterInterceptorHelper
    {
        public static void CallAfter(this MethodToWeave method, Variables variables)
        {
            var caller = new InterceptorCaller(method.Method);

            caller.AddCommonVariables(method.Method.MethodDefinition, variables);
            caller.AddVariable("result", variables.handleResult, true);
            caller.Call(method, variables, configuration => configuration.After);
        }
    }
}