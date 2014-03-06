using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Configuration;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core
{
    public class AssemblyDefinitionProvider : IAssemblyDefinitionProvider
    {
        private readonly Dictionary<Assembly, AssemblyDefinition> asms = new Dictionary<Assembly, AssemblyDefinition>();

        public Dictionary<Assembly, AssemblyDefinition> Asms
        {
            get { return asms; }
        }

        public AssemblyDefinition GetAssemblyDefinition(Assembly assembly)
        {
            if (!asms.ContainsKey(assembly))
            {
                var parameters = new ReaderParameters(ReadingMode.Immediate)
                    {
                        ReadSymbols = true
                    };
                AssemblyDefinition asmDefinition = AssemblyDefinition.ReadAssembly(assembly.GetAssemblyPath(),
                                                                                   parameters);
                asms.Add(assembly, asmDefinition);
            }
            return asms[assembly];
        }
    }
}