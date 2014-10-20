using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables.Instructions
{
    public class VariableCalled : Variable.IVariableBuilder
    {
        private Func<Dictionary<string, VariableDefinition>> calledParametersProvider;

        public VariableCalled(Func<Dictionary<string, VariableDefinition>> calledParametersProvider)
        {
            this.calledParametersProvider = calledParametersProvider;
        }

        public void Check(MethodDefinition method, ErrorHandler errorHandler)
        {
            
        }

        public VariableDefinition Build(InstructionsToInsert instructionsToInsert, MethodDefinition method, Mono.Cecil.Cil.Instruction Instruction)
        {
            calledParametersProvider();
            TypeReference declaringType = null;
            var operand = Instruction.Operand as FieldReference;
            if (operand != null && !operand.Resolve().IsStatic)
                declaringType = operand.DeclaringType;
            var methodReference = Instruction.Operand as MethodReference;
            if (methodReference != null && !methodReference.Resolve().IsStatic)
                declaringType = methodReference.DeclaringType;

            if (declaringType == null)
                return null;

            var called = new VariableDefinition(declaringType);

            instructionsToInsert.calledInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Stloc, called));
            instructionsToInsert.calledInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldloc, called));
            return called;
        }
    }
}