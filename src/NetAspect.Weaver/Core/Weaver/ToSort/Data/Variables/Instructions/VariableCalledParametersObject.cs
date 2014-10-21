using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Helpers.Mono.Cecil.IL;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables.Instructions
{
    public class VariableCalledParametersObject : Variable.IVariableBuilder
    {

        private readonly Func<Dictionary<string, VariableDefinition>> calledParametersProvider;

        public VariableCalledParametersObject(Func<Dictionary<string, VariableDefinition>> calledParametersProvider)
        {
            this.calledParametersProvider = calledParametersProvider;
        }

        public void Check(MethodDefinition method, ErrorHandler errorHandler)
        {
            
        }

        public VariableDefinition Build(InstructionsToInsert instructionsToInsert, MethodDefinition method, Mono.Cecil.Cil.Instruction Instruction)
        {
            Dictionary<string, VariableDefinition> p = calledParametersProvider();
            MethodDefinition calledMethod = Instruction.GetCalledMethod();
            var calledParametersObject = new VariableDefinition(calledMethod.Module.Import(typeof (object[])));

            calledMethod.FillCalledArgsArrayFromParameters(instructionsToInsert.calledParametersObjectInstructions,
                                                           calledParametersObject, p);
            return calledParametersObject;
        }
    }
}