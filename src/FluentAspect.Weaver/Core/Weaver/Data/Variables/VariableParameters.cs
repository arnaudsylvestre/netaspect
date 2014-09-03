using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.Data.Variables
{
    public class VariableParameters : Variable.IVariableBuilder
    {
        public VariableDefinition Build(InstructionsToInsert instructionsToInsert, MethodDefinition method, Instruction instruction)
        {
            var parameters = new VariableDefinition(method.Module.Import(typeof(object[])));

            method.FillArgsArrayFromParameters(instructionsToInsert.BeforeInstructions, parameters);
            return parameters;
        }
    }
}