using System.Collections.Generic;
using Mono.Cecil.Cil;

namespace NetAspect.Weaver.Core.Weaver.Data.Variables
{
   public class VariablesForMethod
   {
       public VariablesForMethod(Variable callerMethod, Variable callerProperty, Variable parameters, Variable exception, Variable result)
       {
           CallerMethod = callerMethod;
           CallerProperty = callerProperty;
           Parameters = parameters;
           Exception = exception;
           Result = result;
       }

       public Variable Parameters { get; private set; }
       public Variable CallerMethod { get; private set; }
       public Variable CallerProperty { get; private set; }
       public Variable Exception { get; private set; }
       public Variable Result { get; private set; }

       public List<Variable> Aspects = new List<Variable>();
   }
}