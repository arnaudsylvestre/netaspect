using System;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Fluent
{
    public static class TypeReferenceExtensions
    {
        public static bool Is(this TypeDefinition type, Type baseType)
        {
            if (type.FullName == baseType.FullName)
                return true;
            if (type.BaseType != null)
                return type.BaseType.Resolve().Is(baseType);
            return false;
        }
         
    }
}