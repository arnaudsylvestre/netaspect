using Mono.Cecil.Cil;

namespace NetAspect.Weaver.Core.Weaver.Data.Variables
{
   public class VariablesForInstruction : VariablesForMethod
   {

       public VariablesForInstruction(Variable callerMethod, Variable callerProperty, Variable parameters, Variable exception, MultipleVariable calledParameters, Variable aspect, Variable called) : base(callerMethod, callerProperty, parameters, exception, aspect)
       {
           CalledParameters = calledParameters;
           Called = called;
       }

       public MultipleVariable CalledParameters { get; private set; }

       public Variable Called { get; set; }
   }
}