using System;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model;
using NetAspect.Weaver.Helpers.Mono.Cecil.IL;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables.Instructions
{
    public class VariableCalledProperty : Variable.IVariableBuilder
    {

        public void Check(MethodDefinition method, ErrorHandler errorHandler)
        {
        }

        public VariableDefinition Build(InstructionsToInsert instructionsToInsert_P, MethodDefinition method, Mono.Cecil.Cil.Instruction instruction)
        {
            VariableDefinition calledProperty = new VariableDefinition(method.Module.Import(typeof(PropertyInfo)));
            var propertyDefinition = instruction.GetOperandAsMethod().GetProperty();
            instructionsToInsert_P.calledPropertyInstructions.AppendCallToTypeOf(method.Module, propertyDefinition.DeclaringType);
            instructionsToInsert_P.calledPropertyInstructions.AppendCallToGetProperty(propertyDefinition.Name, method.Module);
            instructionsToInsert_P.calledPropertyInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Stloc, calledProperty));
            return calledProperty;
        }
    }
}