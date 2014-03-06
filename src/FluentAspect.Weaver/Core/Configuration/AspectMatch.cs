using System;
using System.Collections.Generic;
using FluentAspect.Weaver.Core.Model;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Configuration
{
    public static class AspectMatchExtensions
    {
        public static IEnumerable<AssemblyDefinition> GetAllAssemblies<T>(this IEnumerable<AspectMatch<T>> matches)
        {
            var assemblies_L = new HashSet<AssemblyDefinition>();
            foreach (var match_L in matches)
            {
                foreach (AssemblyDefinition assemblyDefinition_L in match_L.AssembliesToScan)
                {
                    assemblies_L.Add(assemblyDefinition_L);
                }
            }
            return assemblies_L;
        }
    }

    public class AspectMatch<T>
    {
        public IEnumerable<AssemblyDefinition> AssembliesToScan { get; set; }

        public Func<T, bool> Matcher { get; set; }

        public NetAspectDefinition Aspect { get; set; }
    }
}