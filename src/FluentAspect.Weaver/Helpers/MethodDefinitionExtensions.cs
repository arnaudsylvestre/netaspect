using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Helpers
{
    public static class MethodDefinitionExtensions
    {
       public static void InsertAfter(this MethodDefinition method, Instruction instruction, IEnumerable<Instruction> toInsert)
       {
          var instructions = method.Body.Instructions;
          for (int i = 0; i < instructions.Count; i++)
          {
             if (instructions[i] == instruction)
             {
                foreach (Instruction afterInstruction in toInsert.Reverse())
                {
                   instructions.Insert(i + 1, afterInstruction);
                }
                break;
             }
          }
       }
       public static void InsertBefore(this MethodDefinition method, Instruction instruction, IEnumerable<Instruction> toInsert)
       {
          var instructions = method.Body.Instructions;
          for (int i = 0; i < instructions.Count; i++)
          {
             if (instructions[i] == instruction)
             {
                foreach (Instruction beforeInstruction in toInsert.Reverse())
                {
                   instructions.Insert(i, beforeInstruction);
                }
                break;
             }
          }
       }

        public static MethodDefinition Clone(this MethodDefinition methodDefinition, string cloneMethodName)
        {
            MethodAttributes isStatic = (methodDefinition.Attributes & MethodAttributes.Static);

            var wrappedMethod = new MethodDefinition(cloneMethodName, isStatic, methodDefinition.ReturnType);

            foreach (GenericParameter genericParameter in methodDefinition.GenericParameters)
            {
                var genericParameter_L = new GenericParameter(genericParameter.Name, wrappedMethod);
                wrappedMethod.GenericParameters.Add(genericParameter_L);
            }
            foreach (ParameterDefinition parameterDefinition in methodDefinition.Parameters)
            {
                TypeReference parameterType = parameterDefinition.ParameterType;
                if (parameterType is GenericParameter)
                {
                    var genericParameter_L = parameterType as GenericParameter;
                    if (genericParameter_L.Type == GenericParameterType.Method)
                    {
                        parameterType =
                            (from t in wrappedMethod.GenericParameters where t.Name == parameterType.Name select t)
                                .First();
                    }
                }
                wrappedMethod.Parameters.Add(new ParameterDefinition(parameterDefinition.Name,
                                                                     parameterDefinition.Attributes, parameterType));
            }
            foreach (Instruction instruction in methodDefinition.Body.Instructions)
            {
                wrappedMethod.Body.Instructions.Add(instruction);
            }
            foreach (VariableDefinition variable in methodDefinition.Body.Variables)
            {
                wrappedMethod.Body.Variables.Add(variable);
            }
            wrappedMethod.Body.InitLocals = methodDefinition.Body.InitLocals;
            foreach (ExceptionHandler exceptionHandler in methodDefinition.Body.ExceptionHandlers)
            {
                wrappedMethod.Body.ExceptionHandlers.Add(exceptionHandler);
            }
            return wrappedMethod;
        }
    }
}