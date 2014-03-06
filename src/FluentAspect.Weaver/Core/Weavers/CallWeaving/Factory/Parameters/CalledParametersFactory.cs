using System.Collections.Generic;
using System.Linq;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.CallWeaving.Factory.Parameters
{
    public static class CalledParametersFactory
    {
        public static void AddCalledParameters(this ParametersEngine engine, MethodReference reference_P, Dictionary<string, VariableDefinition> variablesForParameters)
        {
            foreach (var parameterDefinition_L in reference_P.Parameters.Reverse())
            {
                engine.AddPossibleParameter((parameterDefinition_L.Name + "Called").ToLower(), (info, handler) =>
                {

                }, (info, instructions) =>
                {
                    VariableDefinition variable = variablesForParameters[info.Name.Substring(0, info.Name.Length - "Called".Length)];
                    instructions.Add(Instruction.Create(OpCodes.Ldloc, variable));
                });
            }
        }
    }
}