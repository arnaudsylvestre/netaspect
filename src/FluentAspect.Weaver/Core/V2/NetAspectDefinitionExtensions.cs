using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Model;

namespace FluentAspect.Weaver.Core.V2
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

        public static IEnumerable<Assembly> GetAssembliesToWeave(this IEnumerable<NetAspectDefinition> aspects_P, Assembly defaultAssembly)
        {
            HashSet<Assembly> assemblies_L = new HashSet<Assembly>();
            assemblies_L.Add(defaultAssembly);
            foreach (var aspect_L in aspects_P)
            {
                var assembliesToWeave = aspect_L.AssembliesToWeave;
                foreach (var assembly in assembliesToWeave)
                {
                    assemblies_L.Add(assembly);

                }
            }
            return assemblies_L;
        }
    }
}