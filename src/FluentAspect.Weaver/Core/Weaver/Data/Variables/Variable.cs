using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Core.Weaver.Data.Variables
{
    public class VariableByAspectType
    {
        private readonly InstructionsToInsert _instructionsToInsertP;
        private VariableAspect variableAspect;
        private readonly MethodDefinition _methodP;
        private readonly Instruction _instruction;
        private readonly List<VariableDefinition> _variables;
        private List<CustomAttribute> customAttributes; 

        public VariableByAspectType(InstructionsToInsert instructionsToInsert_P, VariableAspect variableAspect, MethodDefinition method_P, Instruction instruction, List<VariableDefinition> variables, List<CustomAttribute> customAttributes)
        {
            _instructionsToInsertP = instructionsToInsert_P;
            this.variableAspect = variableAspect;
            _methodP = method_P;
            _instruction = instruction;
            _variables = variables;
            this.customAttributes = customAttributes;
        }

        private Dictionary<Type, List<VariableDefinition>> variables = new Dictionary<Type, List<VariableDefinition>>();

        public List<VariableDefinition> GetAspect(Type type)
        {
            if (!variables.ContainsKey(type))
            {
                variables.Add(type, new List<VariableDefinition>());
                foreach (var attribute in customAttributes)
                {
                    var variableDefinition = variableAspect.Build(_instructionsToInsertP, _methodP, type, attribute);
                    variables[type].Add(variableDefinition);
                    _variables.Add(variableDefinition);
                    
                }
            }
            return variables[type];
        }

        public void Check(ErrorHandler errorHandler, Type type)
        {
            variableAspect.Check(_methodP, errorHandler, type);
        }
    }

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
                   _definition = variableBuilder.Build(instructionsToInsert, method, instruction);
                   variables.Add(_definition);
               }
               return _definition;
           }
       }
   }

}