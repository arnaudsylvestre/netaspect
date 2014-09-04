namespace NetAspect.Weaver.Core.Weaver.Data.Variables
{
   public class VariablesForMethod
   {
       public VariablesForMethod(Variable callerMethod, Variable callerProperty, Variable parameters, Variable exception)
       {
           CallerMethod = callerMethod;
           CallerProperty = callerProperty;
           Parameters = parameters;
           Exception = exception;
       }

       public Variable Parameters { get; private set; }
       public Variable CallerMethod { get; private set; }
       public Variable CallerProperty { get; private set; }
       public Variable Exception { get; private set; }
   }
}