using System.Linq;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Model;
using FluentAspect.Weaver.Core.Weavers.Methods;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Helpers
{
    public static class CallAfterInterceptorHelper
   {
      public static void CallAfter(this MethodToWeave method, Variables variables)
      {
          var caller = new InterceptorCaller(method.Method);

          InterceptorHelpers.Fill(method.Method.MethodDefinition, variables.methodInfo, variables.args, caller);
         caller.AddVariable("result", variables.handleResult, true);

         for (int i = 0; i < method.Interceptors.Count(); i++)
         {
             caller.Call(variables.Interceptors[i], method.Interceptors.ToList()[i].After.Method, method.Interceptors.ToList()[i]);
         }
      } 
   }
}