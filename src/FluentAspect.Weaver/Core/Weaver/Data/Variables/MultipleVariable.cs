using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace NetAspect.Weaver.Core.Weaver.Data.Variables
{
    public class MultipleVariable
    {
        public interface IVariableBuilder
        {
            Dictionary<string, VariableDefinition> Build(InstructionsToInsert instructionsToInsert_P, MethodDefinition method, Instruction instruction);
        }

        private Dictionary<string, VariableDefinition> _definitions = new Dictionary<string, VariableDefinition>();
        private readonly InstructionsToInsert instructionsToInsert;
        private IVariableBuilder variableBuilder;
        private MethodDefinition method;
        private readonly Instruction instruction;

        public MultipleVariable(InstructionsToInsert instructionsToInsert_P, IVariableBuilder variableBuilder_P, MethodDefinition method_P, Instruction instruction)
        {
            instructionsToInsert = instructionsToInsert_P;
            variableBuilder = variableBuilder_P;
            method = method_P;
            this.instruction = instruction;
        }


        public VariableDefinition GetDefinition(string parameterName)
        {
            if (_definitions == null)
            {
                _definitions = variableBuilder.Build(instructionsToInsert, method, instruction);
            }
            return _definitions[parameterName];
        }
    }
}