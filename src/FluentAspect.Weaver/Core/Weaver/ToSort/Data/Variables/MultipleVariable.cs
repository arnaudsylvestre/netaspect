using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables
{
    public class MultipleVariable
    {
        public interface IVariableBuilder
        {
            Dictionary<string, VariableDefinition> Build(InstructionsToInsert instructionsToInsert_P, MethodDefinition method, Mono.Cecil.Cil.Instruction instruction);
        }

        private Dictionary<string, VariableDefinition> _definitions;
        private readonly InstructionsToInsert instructionsToInsert;
        private IVariableBuilder variableBuilder;
        private MethodDefinition method;
        private readonly Mono.Cecil.Cil.Instruction instruction;
        private readonly List<VariableDefinition> _variables;

        public MultipleVariable(InstructionsToInsert instructionsToInsert_P, IVariableBuilder variableBuilder_P, MethodDefinition method_P, Mono.Cecil.Cil.Instruction instruction, List<VariableDefinition> variables)
        {
            instructionsToInsert = instructionsToInsert_P;
            variableBuilder = variableBuilder_P;
            method = method_P;
            this.instruction = instruction;
            _variables = variables;
        }

        public Dictionary<string, VariableDefinition> Definitions
        {
            get
            {
                if (_definitions == null)
                {
                    _definitions = variableBuilder.Build(instructionsToInsert, method, instruction);
                    foreach (var variableDefinition in _definitions.Values)
                    {
                        _variables.Add(variableDefinition);
                    }
                }
                return _definitions;
            }
        } 

        public VariableDefinition GetDefinition(string parameterName)
        {
            return Definitions[parameterName];
        }
    }
}