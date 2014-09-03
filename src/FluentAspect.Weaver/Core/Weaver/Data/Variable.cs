using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace NetAspect.Weaver.Core.Weaver.Data.Variables
{
   public interface IVariableBuilder
   {
      VariableDefinition Build(InstructionsToInsert instructionsToInsert_P, MethodDefinition method);
   }

   public class Variable
   {
      private VariableDefinition _definition;
      private readonly InstructionsToInsert instructionsToInsert;
      private IVariableBuilder variableBuilder;
      private List<VariableDefinition> methodVariables;
      private MethodDefinition method;

      public Variable(InstructionsToInsert instructionsToInsert_P, IVariableBuilder variableBuilder_P, MethodDefinition method_P)
      {
         instructionsToInsert = instructionsToInsert_P;
         variableBuilder = variableBuilder_P;
         method = method_P;
      }


      public VariableDefinition Definition
      {
         get
         {
            if (_definition == null)
            {
               
            }
            return _definition;
         }
      }
   }
}