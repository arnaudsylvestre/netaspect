namespace NetAspect.Weaver.Core.Weaver.Data.Variables
{
   public class VariablesForInstruction : VariablesForMethod
   {
       public VariablesForInstruction(Variable callerMethod, Variable callerProperty, Variable parameters, Variable exception, MultipleVariable calledParameters) : base(callerMethod, callerProperty, parameters, exception)
       {
           CalledParameters = calledParameters;
       }

       public MultipleVariable CalledParameters { get; private set; }
   }
}