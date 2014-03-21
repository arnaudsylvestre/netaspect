using System.Collections.Generic;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.V2.Weaver.Call
{
    public interface IlInstructionInjectorAvailableVariables
    {
        VariableDefinition Parameters { get; }
        Dictionary<Instruction, VariableDefinition> VariablesCalled { get; }
        VariableDefinition Field { get; }
    }
}