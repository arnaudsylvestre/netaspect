using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Tests.Core
{
    public static class TypeDefinitionExtensions
    {
        public static MethodReference GetConstructor(this TypeDefinition type)
        {
            return (from m in type.Methods where m.Name == ".ctor" && m.Parameters.Count == 0 select m).First();
        }
        public static FieldReference FindField(this TypeDefinition type, string fieldName)
        {
            return (from m in type.Fields where m.Name == fieldName select m).First();
        }
        public static VariableDefinition Find(this MethodDefinition method, string variableName)
        {
            return (from m in method.Body.Variables where m.Name == variableName select m).First();
        }
    }
}