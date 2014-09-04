using Mono.Cecil;

namespace NetAspect.Weaver.Core.Weaver.Data.Variables
{
   public class VariablesForMethod
   {
       public VariablesForMethod(Variable callerMethod, Variable callerProperty, Variable parameters, Variable exception, Variable aspect)
       {
           CallerMethod = callerMethod;
           CallerProperty = callerProperty;
           Parameters = parameters;
           Exception = exception;
           Aspect = aspect;
       }

       public Variable Parameters { get; private set; }
       public Variable CallerMethod { get; private set; }
       public Variable CallerProperty { get; private set; }
       public Variable Exception { get; private set; }

       public Variable Aspect { get; private set; }
   }
}