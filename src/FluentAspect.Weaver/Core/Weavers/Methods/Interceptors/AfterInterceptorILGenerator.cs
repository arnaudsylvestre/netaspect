using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.Methods.Interceptors
{
   public class Variables
   {
      public List<VariableDefinition> Interceptors;
      public VariableDefinition methodInfo;
      public VariableDefinition args;
      public VariableDefinition handleResult;
   }

   public class AfterInterceptorILGenerator
   {
      private IEnumerable<MethodWeavingConfiguration> interceptors;

      public AfterInterceptorILGenerator(IEnumerable<MethodWeavingConfiguration> interceptors_P)
      {
         interceptors = interceptors_P;
      }

      public void CallAfter(MethodDefinition method, Variables variables, ILProcessor il)
      {
         var caller = new InterceptorCaller(il, method);

         InterceptorHelpers.FillForBefore(method, variables.methodInfo, variables.args, caller);
         caller.AddVariable("result", variables.handleResult, true);

         for (int i = 0; i < interceptors.Count(); i++)
         {
            caller.Call(variables.Interceptors[i], interceptors.ToList()[i].After.Method, interceptors.ToList()[i]);
         }
      } 
   }
}