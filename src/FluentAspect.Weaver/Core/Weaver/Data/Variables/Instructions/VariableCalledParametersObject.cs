using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.Data.Variables
{
    public class VariableCalledParametersObject : Variable.IVariableBuilder
    {
        public VariableDefinition Build(InstructionsToInsert instructionsToInsert, MethodDefinition method, Instruction Instruction)
        {
            if (Instruction.Operand is MethodReference)
            {
                Dictionary<string, VariableDefinition> p = CalledParameters;
                MethodDefinition calledMethod = Instruction.GetCalledMethod();
                var _calledParametersObject = new VariableDefinition(calledMethod.Module.Import(typeof(object[])));

                calledMethod.FillCalledArgsArrayFromParameters(instructionsToInsert.calledParametersObjectInstructions, _calledParametersObject, p);
            }
        }
    }
}