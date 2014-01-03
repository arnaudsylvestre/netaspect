using System;
using System.Linq;
using FluentAspect.Weaver.Core.Weavers.Helpers;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Model;
using FluentAspect.Weaver.Core.Weavers.Methods;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Helpers
{
    public static class CallOnExceptionInterceptorHelper
   {

      public static void GenerateOnExceptionInterceptor(this MethodToWeave myMethod, Variables variables)
        {
            VariableDefinition e = myMethod.Method.MethodDefinition.CreateVariable(typeof(Exception));
            CallExceptionInterceptor(myMethod, variables, e);
            myMethod.Method.Il.AppendThrow();
        }

      private static void CallExceptionInterceptor(MethodToWeave method, Variables variables, VariableDefinition ex)
      {
          method.Method.Il.Emit(OpCodes.Stloc, ex);
          var caller = new InterceptorCaller(method.Method);

          CallBeforeInterceptorHelper.Fill(method.Method.MethodDefinition, variables.methodInfo, variables.args, caller);
          caller.AddVariable("exception", ex, false);

          caller.Call(method, variables, configuration => configuration.OnException);
      }

   }
}