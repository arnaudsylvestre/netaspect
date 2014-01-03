using System.Linq;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Model;
using FluentAspect.Weaver.Core.Weavers.Methods;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Helpers
{
    public static class CallBeforeInterceptorHelper
   {
      public static void CallBefore(this MethodToWeave method, Variables variables)
      {
          var caller = new InterceptorCaller(method.Method);

          Fill(method.Method.MethodDefinition, variables.methodInfo, variables.args, caller);

          caller.Call(method, variables, configuration => configuration.Before);
      }

      public static void Fill(MethodDefinition method, VariableDefinition methodInfo, VariableDefinition args, InterceptorCaller caller)
      {
          caller.AddThis(Variables.Instance);
          caller.AddVariable(Variables.Method, methodInfo, false);
          caller.AddVariable(Variables.ParameterParameters, args, false);
          caller.AddParameters(method.Parameters);
      }
   }
}