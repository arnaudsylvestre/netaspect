using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Fluent
{
    public static class AssemblyDefinitionExtensions
    {

        public static List<MethodDefinition> GetAllMethods(this AssemblyDefinition assemblyDefinition, Func<MethodDefinition, bool> filter)
        {
            var methods = new List<MethodDefinition>();

            foreach (var type in assemblyDefinition.Modules.SelectMany(module => module.Types))
            {
                methods.AddRange(from m in type.Methods where filter(m) select m);
            }

            return methods;
        }
    }
}