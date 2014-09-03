using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace NetAspect.Weaver.Core.Weaver.Data.Variables.Instructions
{
    public class VariableCalled : Variable.IVariableBuilder
    {
        private Func<Dictionary<string, VariableDefinition>> calledParametersProvider;

        public VariableCalled(Func<Dictionary<string, VariableDefinition>> calledParametersProvider)
        {
            this.calledParametersProvider = calledParametersProvider;
        }

        public VariableDefinition Build(InstructionsToInsert instructionsToInsert, MethodDefinition method, Instruction Instruction)
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

            instructionsToInsert.calledInstructions.Add(Instruction.Create(OpCodes.Stloc, called));
            instructionsToInsert.calledInstructions.Add(Instruction.Create(OpCodes.Ldloc, called));
            return called;
        }
    }
}