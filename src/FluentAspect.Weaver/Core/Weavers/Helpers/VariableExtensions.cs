using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.Helpers
{
    public static class VariableExtensions
    {


        public static VariableDefinition CreateVariable(this MethodDefinition method, Type interceptorType)
        {
            TypeReference typeReference = method.Module.Import(interceptorType);
            return CreateVariable(method, typeReference);
        }


        public static VariableDefinition CreateVariable<T>(this MethodDefinition method)
        {
            return CreateVariable(method, typeof(T));
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