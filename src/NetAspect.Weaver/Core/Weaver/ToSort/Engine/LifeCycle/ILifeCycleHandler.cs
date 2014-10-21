using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Engine.LifeCycle
{
   public interface ILifeCycleHandler
   {
      void CreateInterceptor(Type aspectType,
         MethodDefinition method_P,
         VariableDefinition interceptorVariable,
         List<Mono.Cecil.Cil.Instruction> instructions,
          CustomAttribute attribute);
   }
}
