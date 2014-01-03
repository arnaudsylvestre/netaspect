﻿using System.Collections.Generic;
using System.Linq;
using FluentAspect.Weaver.Core.Weavers.Helpers;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.Methods.Interceptors
{
    public class AfterInterceptorILGenerator
   {
      private IEnumerable<MethodWeavingConfiguration> interceptors;

      public AfterInterceptorILGenerator(IEnumerable<MethodWeavingConfiguration> interceptors_P)
      {
         interceptors = interceptors_P;
      }

      public void CallAfter(Method method, Variables variables)
      {
          var caller = new InterceptorCaller(method.Il, method.MethodDefinition);

          InterceptorHelpers.FillForBefore(method.MethodDefinition, variables.methodInfo, variables.args, caller);
         caller.AddVariable("result", variables.handleResult, true);

         for (int i = 0; i < interceptors.Count(); i++)
         {
            caller.Call(variables.Interceptors[i], interceptors.ToList()[i].After.Method, interceptors.ToList()[i]);
         }
      } 
   }
}