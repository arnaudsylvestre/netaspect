using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Helpers.Mono.Cecil.IL;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables.Instructions
{
   public class VariableCalledMethod : Variable.IVariableBuilder
   {
      private List<VariableDefinition> allVariables;

      public VariableCalledMethod(List<VariableDefinition> allVariables_P)
       {
          allVariables = allVariables_P;
       }

      public void Check(MethodDefinition method, ErrorHandler errorHandler)
        {
            
        }

        public VariableDefinition Build(InstructionsToInsert instructionsToInsert, MethodDefinition method, Mono.Cecil.Cil.Instruction instruction)
        {
           var methodReference = instruction.GetCalledMethod();
           ModuleDefinition module = method.Module;
            VariableDefinition type = new VariableDefinition(module.Import(typeof(Type)));
            allVariables.Add(type);
            VariableDefinition methodVariable = new VariableDefinition(module.Import(typeof(MethodInfo)));
            VariableDefinition called = new VariableDefinition(methodReference.DeclaringType);
            allVariables.Add(called);
            instructionsToInsert.calledConstructorInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Stloc, called));
           instructionsToInsert.calledConstructorInstructions.AppendCallToTypeOf(module, methodReference.DeclaringType);
           instructionsToInsert.calledConstructorInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Stloc, type));
           instructionsToInsert.calledConstructorInstructions.AppendCallToGetMethod(methodReference, module, var => allVariables.Add(var), type);
           instructionsToInsert.calledConstructorInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Stloc, methodVariable));
           instructionsToInsert.calledConstructorInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldloc, called));
           return methodVariable;
        }
   }
}