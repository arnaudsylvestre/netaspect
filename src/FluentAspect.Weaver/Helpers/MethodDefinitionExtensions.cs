using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace NetAspect.Weaver.Helpers
{
    public static class MethodDefinitionExtensions
    {
        public static VariableDefinition CreateVariable(this MethodDefinition method, Type type)
        {
            return CreateVariable(method, method.Module.Import(type));
        }


       public static VariableDefinition CreateVariable(this MethodDefinition method,
                                                        TypeReference typeReference)
        {
            var variableDefinition = new VariableDefinition(typeReference);
            method.Body.Variables.Add(variableDefinition);
            return variableDefinition;
        }
    }
}