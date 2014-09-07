using Mono.Cecil;

namespace NetAspect.Weaver.Core.Weaver.Data.Variables
{
   public class VariablesForMethod
   {
       public VariablesForMethod(Variable callerMethod, Variable callerProperty, Variable parameters, Variable exception, VariableByAspectType aspect, Variable result)
       {
           CallerMethod = callerMethod;
           CallerProperty = callerProperty;
           Parameters = parameters;
           Exception = exception;
           Aspect = aspect;
           Result = result;
       }

       public Variable Parameters { get; private set; }
       public Variable CallerMethod { get; private set; }
       public Variable CallerProperty { get; private set; }
       public Variable Exception { get; private set; }
       public Variable Result { get; private set; }

       public VariableByAspectType Aspect { get; private set; }
   }
}