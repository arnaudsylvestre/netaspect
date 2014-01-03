using FluentAspect.Weaver.Core.Weavers.Methods.Interceptors;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.Methods
{
   public class InterceptorHelpers
   {
      public static void FillForBefore(MethodDefinition method, VariableDefinition methodInfo, VariableDefinition args,
                              InterceptorCaller caller)
      {
         caller.AddThis(Variables.Instance);
         caller.AddVariable(Variables.Method, methodInfo, false);
         caller.AddVariable(Variables.ParameterParameters, args, false);
         caller.AddParameters(method.Parameters);
      }
   }
}