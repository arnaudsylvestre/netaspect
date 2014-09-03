using Mono.Cecil;
using Mono.Cecil.Cil;

namespace NetAspect.Weaver.Core.Weaver.Data.Variables
{
    public class VariableAspect : Variable.IVariableBuilder
    {
        public VariableDefinition Build(InstructionsToInsert instructionsToInsert_P, MethodDefinition method, Instruction instruction)
        {
            throw new System.NotImplementedException();
        }
    }
}