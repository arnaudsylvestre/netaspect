using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace NetAspect.Weaver.Helpers
{
    public static class AssemblyDefinitionExtensions
    {
        public static List<MethodDefinition> GetAllMethods(this AssemblyDefinition assemblyDefinition, Type[] filter)
        {
            return (from moduleDefinition in assemblyDefinition.Modules
                    from typeDefinition in moduleDefinition.GetTypes()
                    where filter == null || filter.FirstOrDefault(t => CompareNames(t, typeDefinition)) != null
                    from methodDefinition in typeDefinition.Methods
                    select methodDefinition).ToList();
        }

        private static bool CompareNames(Type t, TypeDefinition typeDefinition)
        {
            return t.FullName == typeDefinition.FullName.Replace('/', '+');
        }
    }
}