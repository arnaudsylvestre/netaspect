using System;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables.Method
{
    public class VariableException : Variable.IVariableBuilder
    {
        public void Check(MethodDefinition method, ErrorHandler errorHandler)
        {
            
        }

        public VariableDefinition Build(InstructionsToInsert instructionsToInsert_P, MethodDefinition method, Mono.Cecil.Cil.Instruction instruction)
        {
            return new VariableDefinition(method.Module.Import(typeof(Exception)));
        }
    }
}