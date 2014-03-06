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

        public static List<PropertyDefinition> GetAllProperties(this AssemblyDefinition assemblyDefinition)
        {
            return (from moduleDefinition in assemblyDefinition.Modules
                    from typeDefinition in moduleDefinition.GetTypes()
                    from methodDefinition in typeDefinition.Properties
                    select methodDefinition).ToList();
        }

        public static List<FieldDefinition> GetAllFields(this AssemblyDefinition assemblyDefinition)
        {
            return (from moduleDefinition in assemblyDefinition.Modules
                    from typeDefinition in moduleDefinition.GetTypes()
                    from fieldDefinition_L in typeDefinition.Fields
                    select fieldDefinition_L).ToList();
        }
    }
}