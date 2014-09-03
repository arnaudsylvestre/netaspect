using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace NetAspect.Weaver.Core.Weaver.Data.Variables
{
    public class VariableCalled : Variable.IVariableBuilder
    {
        public VariableDefinition Build(InstructionsToInsert instructionsToInsert, MethodDefinition method, Instruction Instruction)
        {
            Dictionary<string, VariableDefinition> calledParameters = CalledParameters;
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