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
       private List<VariableDefinition> methodVariables;
       private MethodDefinition method;
       private readonly Instruction instruction;

       public Variable(InstructionsToInsert instructionsToInsert_P, IVariableBuilder variableBuilder_P, MethodDefinition method_P, Instruction instruction)
       {
           instructionsToInsert = instructionsToInsert_P;
           variableBuilder = variableBuilder_P;
           method = method_P;
           this.instruction = instruction;
       }


       public VariableDefinition Definition
       {
           get
           {
               if (_definition == null)
               {
                   return variableBuilder.Build(instructionsToInsert, method, instruction);

               }
               return _definition;
           }
       }
   }

}