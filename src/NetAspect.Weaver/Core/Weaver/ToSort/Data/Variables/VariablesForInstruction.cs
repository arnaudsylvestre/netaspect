﻿namespace NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables
{
   public class VariablesForInstruction : VariablesForMethod
   {

       public VariablesForInstruction(Mono.Cecil.Cil.Instruction instruction, Variable callerMethod, Variable callerProperty, Variable parameters, Variable exception, MultipleVariable calledParameters, Variable called, Variable fieldValue, Variable result, Variable resultForInstruction, Variable calledParametersObjects)
           : base(callerMethod, callerProperty, parameters, exception, result)
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