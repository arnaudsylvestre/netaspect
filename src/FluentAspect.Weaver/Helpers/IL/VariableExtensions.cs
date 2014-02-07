using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Helpers.IL
{
    public static class VariableExtensions
    {
        public static VariableDefinition CreateVariable(this MethodDefinition method, Type type)
        {
            return CreateVariable(method, method.Module.Import(type));
        }


        public static VariableDefinition CreateVariable<T>(this MethodDefinition method)
        {
            return CreateVariable(method, typeof (T));
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