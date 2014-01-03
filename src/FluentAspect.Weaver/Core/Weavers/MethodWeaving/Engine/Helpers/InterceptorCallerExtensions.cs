using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Model;
using FluentAspect.Weaver.Core.Weavers.Methods;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Helpers
{
    public static class InterceptorCallerExtensions
    {
         public static void AddCommonVariables(this InterceptorCaller caller, MethodDefinition method, Variables variables)
         {
             caller.AddThis(Variables.Instance);
             caller.AddVariable(Variables.Method, variables.methodInfo, false);
             caller.AddVariable(Variables.ParameterParameters, variables.args, false);
             caller.AddParameters(method.Parameters);
         }
    }
}