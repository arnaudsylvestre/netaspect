using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Data
{
   public static class ParameterDefinitionHelper
   {
       


       public static TypeReference ComputeVariableType(ParameterDefinition parameter, Mono.Cecil.Cil.Instruction instruction)
      {
         if (instruction.Operand is GenericInstanceMethod && parameter.ParameterType is GenericParameter)
         {
            var method = (GenericInstanceMethod) instruction.Operand;
            var genericParameter = (GenericParameter) parameter.ParameterType;
            Collection<GenericParameter> genericParameters = ((MethodReference) parameter.Method).GenericParameters;
            int index = -1;
            for (int i = 0; i < genericParameters.Count; i++)
            {
               if (genericParameters[i] == genericParameter)
               {
                  index = i;
                  break;
               }
            }
            if (index != -1)
            {
               return method.GenericArguments[index];
            }
         }
         return parameter.ParameterType;
      }
   }
}
