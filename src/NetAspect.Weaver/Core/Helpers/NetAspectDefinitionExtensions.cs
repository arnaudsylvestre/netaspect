using System.Collections.Generic;
using System.Reflection;
using NetAspect.Weaver.Core.Model.Aspect;

namespace NetAspect.Weaver.Core.Helpers
{
   public static class NetAspectDefinitionExtensions
   {
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
