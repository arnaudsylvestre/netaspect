using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Helpers.Mono.Cecil.IL;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables.Instructions
{
    public class VariablesCalledParameters : MultipleVariable.IVariableBuilder
    {
        public Dictionary<string, VariableDefinition> Build(InstructionsToInsert instructionsToInsert, MethodDefinition method, Mono.Cecil.Cil.Instruction Instruction)
        {
            Dictionary<string, VariableDefinition> _calledParameters = new Dictionary<string, VariableDefinition>();
            if (Instruction.IsAnUpdatePropertyCall())
            {
                MethodDefinition methodDefinition_L = ((MethodReference)Instruction.Operand).Resolve();
                PropertyDefinition property = methodDefinition_L.GetPropertyForSetter();
                _calledParameters = new Dictionary<string, VariableDefinition>();
                TypeReference propertyType_L = property.PropertyType;
                var variableDefinition = new VariableDefinition(propertyType_L);
                _calledParameters.Add("value", variableDefinition);
                instructionsToInsert.calledInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Stloc, variableDefinition));
                instructionsToInsert.recallcalledInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldloc, variableDefinition));
            }
            else if (Instruction.Operand is MethodReference)
            {
                _calledParameters = new Dictionary<string, VariableDefinition>();
                MethodDefinition calledMethod = Instruction.GetCalledMethod();
                foreach (ParameterDefinition parameter in calledMethod.Parameters.Reverse())
                {
                    var variableDefinition = new VariableDefinition(IlInjectorAvailableVariables.ComputeVariableType(parameter, Instruction));
                    _calledParameters.Add("called" + parameter.Name, variableDefinition);
                    instructionsToInsert.calledParametersInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Stloc, variableDefinition));
                }
                foreach (ParameterDefinition parameter in calledMethod.Parameters)
                {
                    instructionsToInsert.recallcalledParametersInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldloc, _calledParameters["called" + parameter.Name]));
                }
            }
            else if (Instruction.IsAnUpdateField())
            {
                _calledParameters = new Dictionary<string, VariableDefinition>();
                TypeReference fieldType = (Instruction.Operand as FieldReference).Resolve().FieldType;
                var variableDefinition = new VariableDefinition(fieldType);
                _calledParameters.Add("value", variableDefinition);
                instructionsToInsert.calledInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Stloc, variableDefinition));
                instructionsToInsert.recallcalledInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldloc, variableDefinition));
            }
            return _calledParameters;
        }
    }
}