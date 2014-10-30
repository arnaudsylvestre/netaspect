using System;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model;
using NetAspect.Weaver.Helpers.Mono.Cecil.IL;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables.Instructions
{
    public class VariableCalledField : Variable.IVariableBuilder
    {

        public void Check(MethodDefinition method, ErrorHandler errorHandler)
        {
        }

        public VariableDefinition Build(InstructionsToInsert instructionsToInsert_P, MethodDefinition method, Mono.Cecil.Cil.Instruction instruction)
        {
            VariableDefinition calledField = new VariableDefinition(method.Module.Import(typeof(FieldInfo)));
            var fieldReference = instruction.GetOperandAsField();
            instructionsToInsert_P.calledPropertyInstructions.AppendCallToTypeOf(method.Module, fieldReference.DeclaringType);
            instructionsToInsert_P.calledPropertyInstructions.AppendCallToGetField(fieldReference.Name, method.Module);
            instructionsToInsert_P.calledPropertyInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Stloc, calledField));
            return calledField;
        }
    }
}