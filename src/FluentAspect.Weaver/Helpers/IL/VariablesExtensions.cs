using System.Collections.Generic;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace NetAspect.Weaver.Helpers.IL
{
   public static class VariablesExtensions
   {
       public static void AddRange(this Collection<VariableDefinition> variables, IEnumerable<VariableDefinition> toAdd)
       {
          foreach (var definition_L in toAdd)
          {
             variables.Add(definition_L);
          }
       }
   }
}