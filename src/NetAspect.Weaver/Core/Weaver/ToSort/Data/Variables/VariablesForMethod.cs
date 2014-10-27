using System.Collections.Generic;
using Mono.Cecil.Cil;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables
{
   public class VariablesForMethod
   {
       private List<VariableDefinition> variablesToAdd; 

       public VariablesForMethod(Variable callerMethod, Variable callerProperty, Variable parameters, Variable exception, Variable result, List<VariableDefinition> variablesToAdd)
       {
           CallerMethod = callerMethod;
           CallerProperty = callerProperty;
           Parameters = parameters;
           Exception = exception;
           Result = result;
           this.variablesToAdd = variablesToAdd;
       }

       public Variable Parameters { get; private set; }
       public Variable CallerMethod { get; private set; }
       public Variable CallerProperty { get; private set; }
       public Variable Exception { get; private set; }
       public Variable Result { get; private set; }

       public List<VariableDefinition> VariablesToAdd
       {
           get { return variablesToAdd; }
       }

       public void AddLocalVariable(VariableDefinition variable)
       {
           variablesToAdd.Add(variable);
       }
   }
}