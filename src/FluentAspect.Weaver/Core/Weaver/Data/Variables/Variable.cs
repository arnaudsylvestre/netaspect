using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace NetAspect.Weaver.Core.Weaver.Data.Variables
{
    public class Variable
   {
       public interface IVariableBuilder
       {

           VariableDefinition Build(InstructionsToInsert instructionsToInsert_P, MethodDefinition method, Instruction instruction);
       }

       private VariableDefinition _definition;
       private readonly InstructionsToInsert instructionsToInsert;
       private IVariableBuilder variableBuilder;
       private List<VariableDefinition> variables;
       private MethodDefinition method;
       private readonly Instruction instruction;

       public Variable(InstructionsToInsert instructionsToInsert_P, IVariableBuilder variableBuilder_P, MethodDefinition method_P, Instruction instruction, List<VariableDefinition> variables)
       {
           instructionsToInsert = instructionsToInsert_P;
           variableBuilder = variableBuilder_P;
           method = method_P;
           this.instruction = instruction;
           this.variables = variables;
       }


       public VariableDefinition Definition
       {
           get
           {
               if (_definition == null)
               {
                   var variableDefinition = variableBuilder.Build(instructionsToInsert, method, instruction);
                   variables.Add(variableDefinition);
                   return variableDefinition;
               }
               return _definition;
           }
       }
   }

}