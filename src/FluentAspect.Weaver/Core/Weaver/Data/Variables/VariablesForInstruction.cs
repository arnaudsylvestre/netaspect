using Mono.Cecil.Cil;

namespace NetAspect.Weaver.Core.Weaver.Data.Variables
{
   public class VariablesForInstruction : VariablesForMethod
   {

       public VariablesForInstruction(Instruction instruction, Variable callerMethod, Variable callerProperty, Variable parameters, Variable exception, MultipleVariable calledParameters, VariableByAspectType aspect, Variable called, Variable fieldValue, Variable result, Variable resultForInstruction, Variable calledParametersObjects)
           : base(callerMethod, callerProperty, parameters, exception, aspect, result)
       {
           Instruction = instruction;
           CalledParameters = calledParameters;
           Called = called;
           FieldValue = fieldValue;
           ResultForInstruction = resultForInstruction;
           CalledParametersObjects = calledParametersObjects;
       }

       public Instruction Instruction { get; private set; }
       public MultipleVariable CalledParameters { get; private set; }

       public Variable Called { get; set; }
       public Variable FieldValue { get; set; }
       public Variable ResultForInstruction { get; set; }

       public Variable CalledParametersObjects { get; set; }
   }
}