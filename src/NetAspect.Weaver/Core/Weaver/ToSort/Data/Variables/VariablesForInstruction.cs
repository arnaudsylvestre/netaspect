using System.Collections.Generic;
using Mono.Cecil.Cil;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables
{
   public class VariablesForInstruction : VariablesForMethod
   {

       public VariablesForInstruction(Mono.Cecil.Cil.Instruction instruction, Variable callerMethod, Variable callerProperty, Variable parameters, Variable exception, MultipleVariable calledParameters, Variable called, Variable result, Variable resultForInstruction, Variable calledParametersObjects, List<VariableDefinition> variables, Variable calledConstructor, Variable calledMethod, Variable calledProperty, Variable calledField)
           : base(callerMethod, callerProperty, parameters, exception, result, variables)
       {
           Instruction = instruction;
           CalledParameters = calledParameters;
           Called = called;
           ResultForInstruction = resultForInstruction;
           CalledParametersObjects = calledParametersObjects;
          CalledConstructor = calledConstructor;
           CalledMethod = calledMethod;
           CalledProperty = calledProperty;
           CalledField = calledField;
       }

       public Mono.Cecil.Cil.Instruction Instruction { get; private set; }
       public MultipleVariable CalledParameters { get; private set; }

       public Variable Called { get; set; }
       public Variable ResultForInstruction { get; set; }

       public Variable CalledParametersObjects { get; set; }
       public Variable CalledConstructor { get; set; }
       public Variable CalledMethod { get; set; }

       public Variable CalledProperty { get; set; }
       public Variable CalledField { get; set; }
   }
}