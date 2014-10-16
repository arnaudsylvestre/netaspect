﻿using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Core.Weaver.Data.Variables.Instructions
{
    public class VariableFieldValue : Variable.IVariableBuilder
    {
        public void Check(MethodDefinition method, ErrorHandler errorHandler)
        {
            
        }

        public VariableDefinition Build(InstructionsToInsert instructionsToInsert, MethodDefinition method, Instruction Instruction)
        {
            FieldDefinition fieldDefinition = ((FieldReference)Instruction.Operand).Resolve();
            TypeReference propertyType_L = fieldDefinition.FieldType;
           var fieldValue = new VariableDefinition(propertyType_L);
            instructionsToInsert.calledInstructions.Add(Instruction.Create(OpCodes.Stloc, fieldValue));
            instructionsToInsert.recallcalledInstructions.Add(Instruction.Create(OpCodes.Ldloc, fieldValue));
            return fieldValue;
        }
    }
}