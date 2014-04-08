using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NetAspect.Weaver.Core.Model.Aspect
{
    public static class NetAspectDefinitionExtensions
    {
        public static List<NetAspectDefinition> FindAspects(IEnumerable<Type> types_P)
        {
            return types_P.
                Select(t => new NetAspectDefinition(t)).
                Where(t => t.IsValid)
                          .ToList();
        }

        public static IEnumerable<Assembly> GetAssembliesToWeave(this IEnumerable<NetAspectDefinition> aspects_P,
                                                                 Assembly defaultAssembly)
        {
            var assemblies_L = new HashSet<Assembly>();
            assemblies_L.Add(defaultAssembly);
            foreach (NetAspectDefinition aspect_L in aspects_P)
            {
                IEnumerable<Assembly> assembliesToWeave = aspect_L.AssembliesToWeave;
                foreach (Assembly assembly in assembliesToWeave)
                {
                    assemblies_L.Add(assembly);
                }
            }
            return assemblies_L;
        }
    }
}