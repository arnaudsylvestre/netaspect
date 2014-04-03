using Mono.Cecil.Cil;

namespace NetAspect.Weaver.Core.Weaver.Call
{
    public interface IlInstructionInjectorAvailableVariables
    {
        VariableDefinition Parameters { get; }
        VariableDefinition Field { get; }
    }
}