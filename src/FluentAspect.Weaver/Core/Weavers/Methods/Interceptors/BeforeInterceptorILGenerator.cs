using System.Collections.Generic;
using System.Linq;
using FluentAspect.Weaver.Core.Weavers.Helpers;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.Methods.Interceptors
{
    public class BeforeInterceptorILGenerator
   {
      private IEnumerable<MethodWeavingConfiguration> interceptors;

      public BeforeInterceptorILGenerator(IEnumerable<MethodWeavingConfiguration> interceptors_P)
      {
         interceptors = interceptors_P;
      }

      public void CallBefore(Method method, Variables variables)
      {
          var caller = new InterceptorCaller(method.Il, method.MethodDefinition);

          InterceptorHelpers.FillForBefore(method.MethodDefinition, variables.methodInfo, variables.args, caller);

          for (int i = 0; i < interceptors.Count(); i++)
          {
              caller.Call(variables.Interceptors[i], interceptors.ToList()[i].Before.Method, interceptors.ToList()[i]);
          }
      }
   }
}