using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.ToSort.Data;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Engine
{
    internal class ExistingVariable : Variable.IVariableBuilder
    {
        private readonly VariableDefinition _variable;

        public ExistingVariable(VariableDefinition variable)
        {
            _variable = variable;
        }

        public void Check(MethodDefinition method, ErrorHandler errorHandler)
        {
            
        }

        public VariableDefinition Build(InstructionsToInsert instructionsToInsert_P, MethodDefinition method, Mono.Cecil.Cil.Instruction instruction)
        {
            return _variable;
        }
    }
}