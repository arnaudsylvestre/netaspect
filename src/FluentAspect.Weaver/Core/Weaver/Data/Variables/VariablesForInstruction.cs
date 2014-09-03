namespace NetAspect.Weaver.Core.Weaver.Data.Variables
{
   public class VariablesForInstruction
   {
       public Variable CallerMethod { get; private set; }
       public Variable CallerProperty { get; private set; }
       public MultipleVariable CalledParameters { get; private set; }
   }
}