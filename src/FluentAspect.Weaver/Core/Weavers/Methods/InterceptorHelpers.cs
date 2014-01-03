using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.Methods
{
   public class InterceptorHelpers
   {
      public static void FillForBefore(MethodDefinition method, VariableDefinition methodInfo, VariableDefinition args,
                              InterceptorCaller caller)
      {
         caller.AddThis(MethodAroundWeaver.Instance);
         caller.AddVariable(MethodAroundWeaver.Method, methodInfo, false);
         caller.AddVariable(MethodAroundWeaver.ParameterParameters, args, false);
         caller.AddParameters(method.Parameters);
      }
   }
}