using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Helpers.Mono.Cecil.IL;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables.Method
{
    public class VariableCurrentMethodBuilder : Variable.IVariableBuilder
    {
        private List<VariableDefinition> methodVariables;

        public VariableCurrentMethodBuilder(List<VariableDefinition> methodVariables)
        {
            this.methodVariables = methodVariables;
        }

        public void Check(MethodDefinition method, ErrorHandler errorHandler)
        {

        }

        public VariableDefinition Build(InstructionsToInsert instructionsToInsert_P, MethodDefinition method, Mono.Cecil.Cil.Instruction instruction)
        {
            var variable = new VariableDefinition(method.Module.Import(method.IsConstructor ? typeof(ConstructorInfo) : typeof(MethodInfo)));
            var type = new VariableDefinition(method.Module.Import(typeof(Type)));
            methodVariables.Add(type);
            instructionsToInsert_P.BeforeInstructions.AppendCallToTypeOf(method.Module, method.DeclaringType);
            instructionsToInsert_P.BeforeInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Stloc, type));
            if (!method.IsConstructor)
            {
                instructionsToInsert_P.BeforeInstructions.AppendCallToGetMethod(method, method.Module, methodVariables.Add, type);

            }
            else
                instructionsToInsert_P.BeforeInstructions.AppendCallToGetConstructor(method, method.Module, methodVariables.Add, type);
            instructionsToInsert_P.BeforeInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Stloc, variable));
            return variable;
        }
    }


   
}