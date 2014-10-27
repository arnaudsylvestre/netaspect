using System.Collections.Generic;
using Mono.Cecil.Cil;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables
{
   public class VariablesForInstruction : VariablesForMethod
   {

       public VariablesForInstruction(Mono.Cecil.Cil.Instruction instruction, Variable callerMethod, Variable callerProperty, Variable parameters, Variable exception, MultipleVariable calledParameters, Variable called, Variable fieldValue, Variable result, Variable resultForInstruction, Variable calledParametersObjects, List<VariableDefinition> variables)
           : base(callerMethod, callerProperty, parameters, exception, result, variables)
       {
           Instruction = instruction;
           CalledParameters = calledParameters;
           Called = called;
           FieldValue = fieldValue;
           ResultForInstruction = resultForInstruction;
           CalledParametersObjects = calledParametersObjects;
       }

       public Mono.Cecil.Cil.Instruction Instruction { get; private set; }
       public MultipleVariable CalledParameters { get; private set; }

       public Variable Called { get; set; }
       public Variable FieldValue { get; set; }
       public Variable ResultForInstruction { get; set; }

       public Variable CalledParametersObjects { get; set; }
   }
}