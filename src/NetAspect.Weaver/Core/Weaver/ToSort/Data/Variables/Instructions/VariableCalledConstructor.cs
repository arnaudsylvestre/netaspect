using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Helpers.Mono.Cecil.IL;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables.Instructions
{
   public class VariableCalledConstructor : Variable.IVariableBuilder
   {
      private List<VariableDefinition> allVariables; 

       public VariableCalledConstructor(List<VariableDefinition> allVariables_P)
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
           VariableDefinition constructorVariable = new VariableDefinition(module.Import(typeof (ConstructorInfo)));
           instructionsToInsert.calledConstructorInstructions.AppendCallToTypeOf(module, methodReference.DeclaringType);
           instructionsToInsert.calledConstructorInstructions.AppendCallToGetConstructor(methodReference, module, var => allVariables.Add(var));
           instructionsToInsert.calledConstructorInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Stloc, constructorVariable));
           return constructorVariable;
        }
   }
}