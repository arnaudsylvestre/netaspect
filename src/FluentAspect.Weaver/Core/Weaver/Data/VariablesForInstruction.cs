using System;
using NetAspect.Weaver.Core.Weaver.Data.Variables;

namespace NetAspect.Weaver.Core.Weaver.Data
{
   public class VariablesForInstruction
   {
       public Variable CallerMethod { get; private set; }
       public Variable CallerProperty { get; private set; }
       public MultipleVariable CalledParameters { get; private set; }
   }
}