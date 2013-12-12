using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Helpers
{
    public static class MethodDefinitionExtensions
    {
        public static MethodDefinition Clone(this MethodDefinition methodDefinition, string cloneMethodName)
        {
            var wrappedMethod = new MethodDefinition(cloneMethodName, methodDefinition.Attributes, methodDefinition.ReturnType);

            foreach (var genericParameter in methodDefinition.GenericParameters)
            {
               var genericParameter_L = new GenericParameter(genericParameter.Name, wrappedMethod);
               wrappedMethod.GenericParameters.Add(genericParameter_L);
            }
            foreach (var parameterDefinition in methodDefinition.Parameters)
            {
               TypeReference parameterType = parameterDefinition.ParameterType;
               if (parameterType is GenericParameter)
               {
                  var genericParameter_L = parameterType as GenericParameter;
                  if (genericParameter_L.Type == GenericParameterType.Method)
                  {
                     parameterType = (from t in wrappedMethod.GenericParameters where t.Name == parameterType.Name select t).First();
                  }
               }
               wrappedMethod.Parameters.Add(new ParameterDefinition(parameterDefinition.Name, parameterDefinition.Attributes, parameterType));
            }
            foreach (var instruction in methodDefinition.Body.Instructions)
            {
                wrappedMethod.Body.Instructions.Add(instruction);
            }
            methodDefinition.Body.Instructions.Clear();
            foreach (var variable in methodDefinition.Body.Variables)
            {
                wrappedMethod.Body.Variables.Add(variable);
            }
            wrappedMethod.Body.InitLocals = methodDefinition.Body.InitLocals;
            methodDefinition.Body.Variables.Clear();
            foreach (var exceptionHandler in methodDefinition.Body.ExceptionHandlers)
            {
                wrappedMethod.Body.ExceptionHandlers.Add(exceptionHandler);
            }
           return wrappedMethod;
        }
         
    }
}