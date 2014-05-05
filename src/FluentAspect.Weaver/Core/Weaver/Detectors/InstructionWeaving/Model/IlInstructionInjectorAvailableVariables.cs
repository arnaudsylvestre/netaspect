using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace NetAspect.Weaver.Core.Weaver.WeavingBuilders.Call
{
    public interface IlInstructionInjectorAvailableVariables
    {
        VariableDefinition Parameters { get; }
        VariableDefinition Field { get; }
        VariableDefinition CurrentMethodBase { get; }
        List<VariableDefinition> Variables { get; }
        List<FieldDefinition> Fields { get; }
    }
}