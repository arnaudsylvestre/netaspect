using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.ToSort.Data;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables.Instructions;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables.Method;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Engine
{
    public static class VariablesFactory
    {

        public static VariablesForMethod CreateVariablesForMethod(InstructionsToInsert instructionsToInsert, MethodDefinition method, List<VariableDefinition> variables, VariableDefinition result)
        {
            return new VariablesForMethod(
                new Variable(instructionsToInsert, new VariableCurrentMethodBuilder(), method, null, variables),
                new Variable(instructionsToInsert, new VariableCurrentProperty(), method, null, variables),
                new Variable(instructionsToInsert, new VariableParameters(), method, null, variables),
                new Variable(instructionsToInsert, new VariableException(), method, null, variables),
                new Variable(instructionsToInsert, new ExistingVariable(result), method, null, variables),
                variables);
        }

        public static VariablesForInstruction CreateVariablesForInstruction(InstructionsToInsert instructionsToInsert, MethodDefinition method, List<VariableDefinition> variables, Mono.Cecil.Cil.Instruction instruction, VariableDefinition result, WeavingMethodSession model)
        {
            VariablesForMethod variablesForMethod = VariablesFactory.CreateVariablesForMethod(instructionsToInsert, method, variables, result);
            var calledParameters = new MultipleVariable(instructionsToInsert, new VariablesCalledParameters(), method, instruction, variables);
           var called_L = new Variable(instructionsToInsert, new VariableCalled(() => calledParameters.Definitions), method, instruction, variables);
           return new VariablesForInstruction(instruction,
                variablesForMethod.CallerMethod,
                variablesForMethod.CallerProperty,
                variablesForMethod.Parameters,
                variablesForMethod.Exception,
                calledParameters,
                called_L,
                new Variable(instructionsToInsert, new VariableFieldValue(), method, instruction, variables),
                variablesForMethod.Result,
                new Variable(instructionsToInsert, new VariableResultForInstruction(), method, instruction, variables),
                new Variable(instructionsToInsert, new VariableCalledParametersObject(() => calledParameters.Definitions), method, instruction, variables),
                variables,
                new Variable(instructionsToInsert, new VariableCalledConstructor(variables), method, instruction, variables));
        }
    }
}