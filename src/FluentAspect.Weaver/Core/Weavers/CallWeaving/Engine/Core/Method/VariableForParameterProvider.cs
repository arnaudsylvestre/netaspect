using System.Collections.Generic;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.CallWeaving.Factory
{
    class VariableForParameterProvider
    {
        Dictionary<string, VariableDefinition> variables = new Dictionary<string, VariableDefinition>(); 

        public VariableDefinition GetVariableForParameter(string parameterName)
        {
            return variables[parameterName];
        }

        public void SaveParameterInVariable()
        {
                
        }
    }
}