using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Helpers.Mono.Cecil.IL;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables.Instructions
{
    public class VariableResultForInstruction : Variable.IVariableBuilder
    {
        public void Check(MethodDefinition method, ErrorHandler errorHandler)
        {
            
        }

        public VariableDefinition Build(InstructionsToInsert instructionsToInsert, MethodDefinition methodToWeave, Mono.Cecil.Cil.Instruction Instruction)
        {
            VariableDefinition _resultForInstruction = null;
            if (Instruction.Operand is MethodReference)
            {
                var method = Instruction.Operand as MethodReference;
                if (method.ReturnType == method.Module.TypeSystem.Void)
                    return null;
                _resultForInstruction = new VariableDefinition(method.ReturnType);
                instructionsToInsert.resultInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Stloc, _resultForInstruction));
                instructionsToInsert.resultInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldloc, _resultForInstruction));
            }
            if (Instruction.IsAGetField())
            {
                var fieldReference_L = Instruction.Operand as FieldReference;
                _resultForInstruction = new VariableDefinition(fieldReference_L.FieldType);
                instructionsToInsert.resultInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Stloc, _resultForInstruction));
                instructionsToInsert.resultInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldloc, _resultForInstruction));
            }
            return _resultForInstruction;
        }
    }
}