using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.Data.Variables.Instructions
{
    public class VariableResultForInstruction : Variable.IVariableBuilder
    {
        public void Check(MethodDefinition method, ErrorHandler errorHandler)
        {
            
        }

        public VariableDefinition Build(InstructionsToInsert instructionsToInsert, MethodDefinition methodToWeave, Instruction Instruction)
        {
            VariableDefinition _resultForInstruction = null;
            if (Instruction.Operand is MethodReference)
            {
                var method = Instruction.Operand as MethodReference;
                if (method.ReturnType == method.Module.TypeSystem.Void)
                    return null;
                _resultForInstruction = new VariableDefinition(method.ReturnType);
                instructionsToInsert.resultInstructions.Add(Instruction.Create(OpCodes.Stloc, _resultForInstruction));
                instructionsToInsert.resultInstructions.Add(Instruction.Create(OpCodes.Ldloc, _resultForInstruction));
            }
            if (Instruction.IsAGetField())
            {
                var fieldReference_L = Instruction.Operand as FieldReference;
                _resultForInstruction = new VariableDefinition(fieldReference_L.FieldType);
                instructionsToInsert.resultInstructions.Add(Instruction.Create(OpCodes.Stloc, _resultForInstruction));
                instructionsToInsert.resultInstructions.Add(Instruction.Create(OpCodes.Ldloc, _resultForInstruction));
            }
            return _resultForInstruction;
        }
    }
}