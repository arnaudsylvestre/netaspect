using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Helpers.Mono.Cecil.IL;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables.Method
{
    public class VariableParameters : Variable.IVariableBuilder
    {
        public void Check(MethodDefinition method, ErrorHandler errorHandler)
        {
            
        }

        public VariableDefinition Build(InstructionsToInsert instructionsToInsert, MethodDefinition method, Mono.Cecil.Cil.Instruction instruction)
        {
            var parameters = new VariableDefinition(method.Module.Import(typeof(object[])));

            method.FillArgsArrayFromParameters(instructionsToInsert.BeforeInstructions, parameters);
            return parameters;
        }
    }
}