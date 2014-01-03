using System;
using System.Collections.Generic;
using System.Linq;
using FluentAspect.Weaver.Core.Weavers.Helpers;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.Methods.Interceptors
{
    public class OnExceptionInterceptorILGenerator
   {
      private IEnumerable<MethodWeavingConfiguration> interceptors;

      public OnExceptionInterceptorILGenerator(IEnumerable<MethodWeavingConfiguration> interceptors_P)
      {
         interceptors = interceptors_P;
      }

      public void CallExceptionInterceptor(Method method, Variables variables, VariableDefinition ex)
      {
          method.Il.Emit(OpCodes.Stloc, ex);
          var caller = new InterceptorCaller(method.Il, method.MethodDefinition);

          InterceptorHelpers.FillForBefore(method.MethodDefinition, variables.methodInfo, variables.args, caller);
          caller.AddVariable("exception", ex, false);

          for (int i = 0; i < interceptors.Count(); i++)
          {
              caller.Call(variables.Interceptors[i], interceptors.ToList()[i].OnException.Method, interceptors.ToList()[i]);
          }
      }

      public void GenerateOnExceptionInterceptor(Method myMethod, Variables variables)
        {
            VariableDefinition e = myMethod.MethodDefinition.CreateVariable(typeof(Exception));
            CallExceptionInterceptor(myMethod, variables, e);
            myMethod.Il.AppendThrow();
        }
   }
}