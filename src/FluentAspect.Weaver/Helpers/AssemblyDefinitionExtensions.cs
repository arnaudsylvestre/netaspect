using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace FluentAspect.Weaver.Helpers
{
    public static class AssemblyDefinitionExtensions
    {
        public static List<MethodDefinition> GetAllMethods(this AssemblyDefinition assemblyDefinition)
        {
            return (from moduleDefinition in assemblyDefinition.Modules
                    from typeDefinition in moduleDefinition.GetTypes()
                    from methodDefinition in typeDefinition.Methods
                    select methodDefinition).ToList();
        }
    }
}